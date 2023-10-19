using Bankomat_simulator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Bankomat_simulator
{
    public class InterfacciaUtente
    {
        private static string connectionString = "data source=.; initial catalog=soluzione_bankomat; User ID=sa; Password=password123; Trusted_Connection=true; TrustServerCertificate=true";

        private static List<Banca> banche = new List<Banca>();
        private static List<Utente> utenti = new List<Utente>();
        public static string CaricamentoBanche()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var max = 0;

                string query = "SELECT Id, Nome FROM Banche ORDER BY id";
                SqlCommand cmd = new SqlCommand(query, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int Id = (int) reader.GetInt64(0);
                        string Nome = reader.GetString(1);

                        banche.Add(new Banca()
                        {
                            Id = Id,
                            Nome = Nome
                        });

                        Console.WriteLine($"{Id} - {Nome}");

                        max++;
                    }
                }
                Console.WriteLine("0 - per uscire");
                //string scelta = Console.ReadKey().KeyChar.ToString();
                string nome = Controllo(0, max);
                return nome;
            }
        }

        public static void MenuIntestazione(string nomeMenu)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("    BENVENUTO NEL BANKOMAT SIMULATOR");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("".PadLeft((40 - nomeMenu.Length) / 2) + nomeMenu);
            Console.WriteLine();
        }

        public static string Controllo(int min, int max)
        {
            string rispostaUtente;


            Console.Write("scelta: ");
            rispostaUtente = Console.ReadKey().KeyChar.ToString();
            if (!Int32.TryParse(rispostaUtente, out int scelta) ||
                !(min <= scelta && scelta <= max))
            {
                scelta = -1;
                Console.WriteLine("");
                Console.WriteLine($"Scelta non consentita - {rispostaUtente}");
                Console.Write("Premere un tasto per proseguire");
                Console.ReadKey();
            }

            string nome = RestituisciBanca(scelta);

            return nome; 
        }
        public static void SceltaBanche()
        {
            string nome = CaricamentoBanche();
            //MenuIntestazione("login - "+nome);
            Login(nome);
        }

        public static string RestituisciBanca(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Nome FROM Banche WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                object result = cmd.ExecuteScalar();

                string nome = result.ToString();

                //string query2 = "SELECT * FROM Utenti WHERE IdBanca = @id";
                //SqlCommand cmd2 = new SqlCommand(query, connection);
                //cmd2.Parameters.AddWithValue("@id", id);


                return nome;
            }
        }

        public static void Login(string nomeBanca)
        {
            int error = 0;
            do {
                MenuIntestazione(nomeBanca);
                Console.WriteLine("Username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Utenti WHERE NomeUtente = @nomeutente AND Password = @password";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@nomeutente", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();

                    int userCount = 0;

                    if (result != null)
                    {
                        userCount = Convert.ToInt32(result);
                    }

                    if (userCount == 1)
                    {
                        ControlloPermessi();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("\nusername o password errati");
                        error++;
                        Console.WriteLine("premi un tasto per continuare: ");
                        Console.ReadKey();
                    }
                        
                }
            }while(error > 0);
            MenuIntestazione($"Benvenuto - {nomeBanca}");
        }

        public static void ControlloPermessi()
        {
        //    Console.WriteLine("sa");
        }
        
    }
}
