using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_Project4.Views;

namespace WpfApp_Project4.Models
{
    public class Project4db
    {
        #region Messages
        public static readonly string UNKNOWN = "Unknown";
        public static readonly string OK = "Ok";
        public static readonly string NOTFOUND = "not found";
        #endregion

        private readonly string connString = ConfigurationManager.ConnectionStrings["project4"].ConnectionString;



        public string GetBestellingen(ICollection<Bestelling> bestellingen)
        {
            if (bestellingen == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetBestellingen");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT b.bestellingID, b.klantID, b.datum, b.statusId , p.id, p.first_name, p.last_name, p.postal_code, p.address, p.phone, p.personal_email, s.statusId, s.statusNaam
                        FROM bestellingen b
                        INNER JOIN people p ON p.id = b.klantId
                        INNER JOIN bestelstatus s ON s.statusId = b.statusId
                        ORDER BY b.bestellingID
                        ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Bestelling bestelling = new Bestelling()
                        {
                            BestellingId = (int)reader["bestellingID"],
                            KlantId = (ulong)reader["klantID"],
                            Datum = (DateTime)reader["datum"],
                            KlantId2 = (ulong)reader["id"],
                            Klant = new Klant()
                            {
                                KlantId2 = (ulong)reader["id"],
                                VoorNaam = (string)reader["first_name"],
                                AchterNaam = (string)reader["last_name"],
                                PostCode = (string)reader["postal_code"],
                                Adres = (string)reader["address"],
                                Nummer = (string)reader["phone"],
                                Mail = (string)reader["personal_email"],

                            },
                            StatusId = (int)reader["statusId"],
                            BestelStatus = new BestelStatus()
                            {
                                StatusId = (int)reader["statusId"],
                                StatusNaam = (string)reader["statusNaam"],
                            }
                        };
                        GetTotaalPrijs(bestelling.BestellingId, bestelling.Regel);

                        bestellingen.Add(bestelling);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetBestellingen));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetTotaalPrijs(int bestellingId, ICollection<Bestelregel> bestelRegels)
        {
            if (bestelRegels == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetBestelRegelsByBestelling");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                            SELECT br.bestelregelID, br.bestellingID, br.productId, br.aantal, p.id as 'ProductId', p.name, p.price
                            FROM bestelregels br
                            INNER JOIN products p ON p.id = br.productId
                            WHERE br.bestellingID = @bestellingID
                        ";
                    sql.Parameters.AddWithValue("@bestellingID", bestellingId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Bestelregel bestelRegel = new()
                        {
                            BestelRegelId = (int)reader["bestelregelID"],
                            BestellingId = (int)reader["bestellingID"],
                            ProductId = (int)reader["ProductId"],
                            Product = new()
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductPrijs = (decimal)reader["price"],
                            },
                            Aantal = (int)reader["aantal"],
                        };
                        bestelRegels.Add(bestelRegel);
                    }
                    methodResult = OK;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetTotaalPrijs));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }



        public string GetBestelRegelsByBestelling(int bestellingId, ICollection<Bestelregel> bestelRegels)
        {
            if (bestelRegels == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetBestelRegelsByBestelling");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                            SELECT br.bestelregelID, br.bestellingID, br.productId, br.aantal, p.id as 'ProductId', p.name, p.price, a.afmetingId, a.afmetingNaam
                            FROM bestelregels br
                            INNER JOIN products p ON p.id = br.productId
                            INNER JOIN afmeting a ON a.afmetingId = br.afmetingId
                            WHERE br.bestellingID = @bestellingID
                        ";
                    sql.Parameters.AddWithValue("@bestellingID", bestellingId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Bestelregel bestelRegel = new()
                        {
                            BestelRegelId = (int)reader["bestelregelID"],
                            BestellingId = (int)reader["bestellingID"],
                            ProductId = (int)reader["ProductId"],
                            Product = new()
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["name"],
                                ProductPrijs = (decimal)reader["price"],
                            },
                            Aantal = (int)reader["aantal"],
                            AfmetingId = (int)reader["afmetingId"],
                            Afmeting = new()
                            {
                                AfmetingId = (int)reader["afmetingId"],
                                AfmetingNaam = (string)reader["afmetingNaam"],
                            }
                        };

                        bestelRegels.Add(bestelRegel);
                    }
                    methodResult = OK;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetBestelRegelsByBestelling));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetStatuses(ICollection<BestelStatus> statuses)
        {
            if (statuses == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetStatuses");
            }
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT statusId, statusNaam
                        FROM bestelstatus
                        ORDER BY statusId
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        BestelStatus status = new()
                        {
                            StatusId = (int)reader["statusId"],
                            StatusNaam = (string)reader["statusNaam"],

                        };
                        statuses.Add(status);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetStatuses));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string UpdateStatus(int BestellingId, int statusId, BestelStatus status)
        {
            if (status == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateStatus");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        UPDATE bestellingen
                        SET statusId = @newId
                        WHERE bestellingId = @bestellingId;
                        ";
                    sql.Parameters.AddWithValue("@newId", statusId);
                    sql.Parameters.AddWithValue("@bestellingId", BestellingId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"De status {status.StatusNaam} kon niet gewijzigd worden.{statusId} {status.StatusId}";

                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateStatus));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteBestelling(int BestellingId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE FROM bestellingen
                        WHERE bestellingId = @id;
                    ";
                    sql.Parameters.AddWithValue("@id", BestellingId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Bestelling met orderId {BestellingId} kon niet worden verwijderd.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteBestelling));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }

            return methodResult;
        }

        public string DeleteBestelRegels(int bestellingId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE FROM bestelregels
                        WHERE bestellingID = @bestellingId;
                    ";
                    sql.Parameters.AddWithValue("@bestellingId", bestellingId);

                    if (sql.ExecuteNonQuery() >= 0)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = "Fout bij het verwijderen van de bestelregels.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteBestelRegels));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }

            return methodResult;
        }



        public string GetUnits(ICollection<Unit> units)
        {
            if (units == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetUnits");
            }
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT id as unitId, name as unitNaam
                        FROM units
                        ORDER BY name
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Unit unit = new()
                        {
                            UnitId = (int)reader["unitId"],
                            UnitNaam = (string)reader["unitNaam"],

                        };
                        units.Add(unit);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetUnits));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetUnit(int unitId, out Unit? unit)
        {
            unit = null;
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT id as unitId, name as unitNaam
                        FROM ingredients
                        WHERE unitId = @unitId;
                        ";
                    sql.Parameters.AddWithValue("@unitId", unitId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        unit = new()
                        {
                            UnitId = (int)reader["unitId"],
                            UnitNaam = (string)reader["UnitNaam"],
                        };
                    }

                    methodResult = unit == null ? NOTFOUND : OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string CreateUnit(Unit unit)
        {
            if (unit == null || string.IsNullOrEmpty(unit.UnitNaam))
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateUnit");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    INSERT INTO units 
                            (id, name) 
                    VALUES  (NULL, @name);
                    ";
                    sql.Parameters.AddWithValue("@name", unit.UnitNaam);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit {unit.UnitNaam} kon niet toegevoegd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string UpdateUnit(int unitId, Unit unit)
        {
            if (unit == null || string.IsNullOrEmpty(unit.UnitNaam))
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateUnit");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        UPDATE units
                        SET name = @name
                        WHERE id = @id;
                        ";
                    sql.Parameters.AddWithValue("@id", unitId);
                    sql.Parameters.AddWithValue("@name", unit.UnitNaam);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit {unit.UnitNaam} kon niet gewijzigd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteUnit(int unitId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE 
                        FROM units 
                        WHERE id = @id 
                    ";
                    sql.Parameters.AddWithValue("@id", unitId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Unit met id {unitId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteUnit));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }



        public string GetIngredients(ICollection<Ingredient> ingredients)
        {
            if (ingredients == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetIngredients");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT i.id, i.name, i.price, i.unit_id, u.name as unitName
                        FROM ingredients i
                        INNER JOIN units u ON u.id = i.unit_id
                        ORDER BY i.name
                    ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Ingredient ingredient = new()
                        {
                            IngredientId = (int)reader["id"],
                            Name = (string)reader["name"],
                            IngredientPrijs = (decimal)reader["price"],
                            UnitId = (int)reader["unit_id"],
                            Unit = new Unit()
                            {
                                UnitId = (int)reader["id"],
                                UnitNaam = (string)reader["unitName"],

                            }
                        };
                        ingredients.Add(ingredient);
                    }
                    methodResult = OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetIngredients));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetIngredient(int ingredientId, out Ingredient? ingredient)
        {
            ingredient = null;
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT i.id, i.name, i.price, i.unit_id, u.name as unitName
                        FROM ingredients i
                        INNER JOIN units u ON u.id = i.unit_id
                        WHERE i.id = @ingredientId;
                        ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        ingredient = new()
                        {
                            IngredientId = (int)reader["ingredientId"],
                            Name = (string)reader["name"],
                            IngredientPrijs = (decimal)reader["price"],
                            UnitId = (int)reader["unit_id"],
                            Unit = new Unit()
                            {
                                UnitId = (int)reader["id"],
                                UnitNaam = (string)reader["unitName"],

                            }
                        };
                    }

                    methodResult = ingredient == null ? NOTFOUND : OK;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string CreateIngredient(Ingredient ingredient)
        {
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.IngredientPrijs < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                    INSERT INTO ingredients 
                            (id,  name,  price,  unit_id) 
                    VALUES  (NULL,         @name, @price, @unitId);
                    ";
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.IngredientPrijs);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingrediënt {ingredient.Name} kon niet toegevoegd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string UpdateIngredient(int ingredientId, Ingredient ingredient)
        {
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)
                || ingredient.IngredientPrijs < 0 || ingredient.UnitId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van UpdateIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        UPDATE ingredients
                        SET name = @name, 
                            price = @price,
                            unit_id = @unitId
                        WHERE id = @ingredientId;
                        ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    sql.Parameters.AddWithValue("@name", ingredient.Name);
                    sql.Parameters.AddWithValue("@price", ingredient.IngredientPrijs);
                    sql.Parameters.AddWithValue("@unitId", ingredient.UnitId);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingredient {ingredient.Name} kon niet gewijzigd worden.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(UpdateIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteIngredient(int ingredientId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE 
                        FROM ingredients 
                        WHERE id = @ingredientId 
                    ";
                    sql.Parameters.AddWithValue("@ingredientId", ingredientId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingredient met id {ingredientId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }



        public string GetProducten(ICollection<Product> producten)
        {
            if (producten == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetProducten");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        SELECT id, name
                        FROM products
                        ";
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        Product meal = new Product()
                        {
                            ProductId = (int)reader["id"],
                            ProductName = (string)reader["name"],
                        };
                        meal.ProductIngredients = new List<ProductIngredient>();
                        GetProductIngredientsByProduct(meal.ProductId, meal.ProductIngredients);

                        producten.Add(meal);
                    }
                    methodResult = OK;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetProducten));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string GetProductIngredientsByProduct(int productId, ICollection<ProductIngredient> productIngredients)
        {
            if (productIngredients == null)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van GetProductIngredientsByProduct");
            }

            string methodResult = UNKNOWN;


            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                            SELECT pi.id as 'productIngredientId', pi.productId, pi.ingredientId, pi.aantalIngr, 
                                   i.id as 'IngrID', i.name, i.price, i.unit_id, 
                                   u.id as 'UnID', u.name as 'unitNaam'
                            FROM ingredient_product pi
                            INNER JOIN ingredients i ON i.id = pi.ingredientId
                            INNER JOIN units u ON u.id = i.unit_id
                            WHERE pi.productId = @productId
                        ";
                    sql.Parameters.AddWithValue("@productId", productId);
                    MySqlDataReader reader = sql.ExecuteReader();

                    while (reader.Read())
                    {
                        ProductIngredient productIngredient = new()
                        {
                            ProductIngredientId = (int)reader["productIngredientId"],
                            ProductId = (int)reader["productId"],
                            IngredientId = (int)reader["ingredientId"],
                            AantalIngr = (uint)reader["aantalIngr"],
                            Ingredient = new()
                            {
                                IngredientId = (int)reader["IngrID"],
                                Name = (string)reader["name"],
                                IngredientPrijs = (decimal)reader["price"],
                                UnitId = (int)reader["unit_id"],
                                Unit = new()
                                {
                                    UnitId = (int)reader["UnID"],
                                    UnitNaam = (string)reader["UnitNaam"]
                                }
                            }
                        };
                        productIngredients.Add(productIngredient);
                    }
                    methodResult = OK;

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(GetProductIngredientsByProduct));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string CreateProductIngredient(ProductIngredient productIngredient)
        {
            if (productIngredient == null
                || productIngredient.AantalIngr == 0
                || productIngredient.ProductId == 0
                || productIngredient.IngredientId == 0)
            {
                throw new ArgumentException("Ongeldig argument bij gebruik van CreateProductIngredient");
            }

            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        INSERT INTO ingredient_product
		                        (id,   productId,  ingredientId,  aantalIngr) 
                        VALUES  (NULL, @productId, @ingredientId, @aantalIngr) 
                    ";
                    sql.Parameters.AddWithValue("@productId", productIngredient.ProductId);
                    sql.Parameters.AddWithValue("@ingredientId", productIngredient.IngredientId);
                    sql.Parameters.AddWithValue("@aantalIngr", productIngredient.AantalIngr);

                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Ingrediënt {productIngredient.IngredientId} kon niet toegevoegd " +
                            $"worden aan maaltijd {productIngredient.ProductId}.";
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(CreateProductIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

        public string DeleteProductIngredient(int productIngredientId)
        {
            string methodResult = UNKNOWN;

            using (MySqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand sql = conn.CreateCommand();
                    sql.CommandText = @"
                        DELETE FROM ingredient_product
                        WHERE id = @productIngredientId
                    ";
                    sql.Parameters.AddWithValue("@productIngredientId", productIngredientId);
                    if (sql.ExecuteNonQuery() == 1)
                    {
                        methodResult = OK;
                    }
                    else
                    {
                        methodResult = $"Productingredient met id {productIngredientId} kon niet verwijderd worden.";
                    }

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(nameof(DeleteProductIngredient));
                    Console.Error.WriteLine(e.Message);
                    methodResult = e.Message;
                }
            }
            return methodResult;
        }

    }
}
