using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Npgsql;

namespace NpgsqlIntroduction
{
    class Program
    {
        static void Main(string[] args)
        {
            // Specify connection options and open an connection		
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["Rental"].ToString();
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();









            while (true)
            {
                Console.WriteLine("(1) Current Offers");
                Console.WriteLine("(2) All rentals");
                Console.WriteLine("(3) New Rental ");
                Console.WriteLine("(4) Registering a return a copy");
                Console.WriteLine("(5) Create of a new user ");
                Console.WriteLine("(6) Create a new movie ");
                Console.WriteLine("(7) Overdue rentals ");
                Console.WriteLine("(0) Exit");

                string menu = Console.ReadLine();

                switch (menu)
                {
                    case "1":
                        {
                            // Define a query
                            NpgsqlCommand cmd = new NpgsqlCommand("SELECT movies.title,movies.movie_id,copies.available, COUNT (movies.movie_id) FROM movies, copies WHERE movies.movie_id = copies.movie_id AND copies.available='1' GROUP BY movies.movie_id,copies.available", conn);
                            NpgsqlDataReader dataReader = cmd.ExecuteReader();
                            while (dataReader.Read())
                                Console.Write($"Movie: {dataReader[0]} --> {dataReader[3]} copies available \n");
                            // Once we're done reading data we need to close the reader
                            dataReader.Close();
                        }

                        break;

                    case "2":
                        {

                            Console.WriteLine("Please enter the client's 'last_name'");
                            string client_lastname = Console.ReadLine();
                            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT client_id,first_name,last_name FROM clients WHERE last_name='{client_lastname}'\n", conn);
                            NpgsqlDataReader cmd2 = cmd.ExecuteReader();
                            while (cmd2.Read())
                                Console.Write($"Client_ID: {cmd2[0]} --> Name: {cmd2[1]} --> Last Name: {cmd2[2]}  \n");
                            // Once we're done reading data we need to close the reader
                            cmd2.Close();


                            // Define a query
                            Console.WriteLine("Enter the 'Client_ID'");
                            int user_id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("(1) Active Rentals");
                            Console.WriteLine("(2) Historic Rentals");
                            string choose = Console.ReadLine();
                            if (choose == "1")
                            {
                                NpgsqlCommand active_rentals = new NpgsqlCommand($"SELECT rentals.*,clients.* FROM rentals,clients where clients.client_id = rentals.client_id and clients.client_id ={user_id} and return = '0'", conn);
                                NpgsqlDataReader active_rentals2 = active_rentals.ExecuteReader();
                                while (active_rentals2.Read())
                                    Console.Write($"Name and Surname : {active_rentals2[7]}-{active_rentals2[8]} --- Date of rental: {active_rentals2[2]} --- Date of Return:  {active_rentals2[3]} \n");
                                // Once we're done reading data we need to close the reader
                                active_rentals2.Close();

                            }
                            else if (choose == "2")
                            {

                                NpgsqlCommand historic_rentals = new NpgsqlCommand($"SELECT rentals.*,clients.* FROM rentals,clients where clients.client_id = rentals.client_id and clients.client_id ={user_id} and return = '1'", conn);
                                NpgsqlDataReader historic_rentals2 = historic_rentals.ExecuteReader();
                                while (historic_rentals2.Read())
                                    Console.Write($"Name and Surname : {historic_rentals2[7]}-{historic_rentals2[8]} --- Date of rental: {historic_rentals2[2]} --- Date of Return:  {historic_rentals2[3]} \n");
                                // Once we're done reading data we need to close the reader
                                historic_rentals2.Close();


                            }
                        }
                        break;

                    case "3":
                        {
                            Console.WriteLine("Please enter the client's 'last_name'");
                            string client_lastname = Console.ReadLine();
                            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT client_id,first_name,last_name FROM clients WHERE last_name='{client_lastname}'\n", conn);
                            NpgsqlDataReader cmd2 = cmd.ExecuteReader();
                            while (cmd2.Read())
                                Console.Write($"Client_ID: {cmd2[0]} --> Name: {cmd2[1]} --> Last Name: {cmd2[1]} copies available \n");
                            // Once we're done reading data we need to close the reader
                            cmd2.Close();
                            Console.WriteLine("Enter the 'Client_ID'");
                            int client_id = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Available Rentals: ");
                            NpgsqlCommand available_rentals = new NpgsqlCommand("SELECT movies.title,movies.movie_id,copies.available,copies.copy_id FROM movies, copies WHERE movies.movie_id = copies.movie_id AND copies.available='1'", conn);
                            NpgsqlDataReader available_rentals2 = available_rentals.ExecuteReader();
                            while (available_rentals2.Read())
                                Console.Write($"Movie: {available_rentals2[0]} --> Copy_ID: {available_rentals2[3]}\n");
                            // Once we're done reading data we need to close the reader
                            available_rentals2.Close();
                            Console.WriteLine("Enter the movies's 'Copy_ID'");
                            int copy_id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the return date: yyyy-mm-dd");
                            string return_date = Console.ReadLine();

                            NpgsqlCommand new_rental = new NpgsqlCommand($"INSERT INTO rentals (copy_id, client_id,date_of_rental,date_of_return,return) VALUES ({copy_id},  {client_id}, CURRENT_TIMESTAMP, '{return_date}',false); ", conn);
                            NpgsqlCommand copies = new NpgsqlCommand($"UPDATE copies SET available = '0' WHERE copy_id={copy_id} ", conn);
                            new_rental.ExecuteNonQuery();
                            copies.ExecuteNonQuery();
                            Console.WriteLine("New rental added and current offers");


                        }
                        break;

                    case "4":
                        {
                            Console.WriteLine("Please enter the client's 'last_name'");
                            string client_lastname = Console.ReadLine();
                            NpgsqlCommand cmd = new NpgsqlCommand($"SELECT client_id,first_name,last_name FROM clients WHERE last_name='{client_lastname}'\n", conn);
                            NpgsqlDataReader cmd2 = cmd.ExecuteReader();
                            while (cmd2.Read())
                                Console.Write($"Client_ID: {cmd2[0]} --> Name: {cmd2[1]} --> Last Name: {cmd2[2]}  \n");
                            // Once we're done reading data we need to close the reader
                            cmd2.Close();

                            // Define a query
                            Console.WriteLine("Enter the 'Client_ID'");
                            int user_id = Convert.ToInt32(Console.ReadLine());

                            NpgsqlCommand active_rentals = new NpgsqlCommand($"SELECT rentals.*,clients.* FROM rentals,clients where clients.client_id = rentals.client_id and clients.client_id ={user_id} and return = '0'", conn);
                            NpgsqlDataReader active_rentals2 = active_rentals.ExecuteReader();
                            while (active_rentals2.Read())
                                Console.Write($"Name and Surname : {active_rentals2[7]}-{active_rentals2[8]} ---> Date of rental: {active_rentals2[2]} ---> Date of Return:  {active_rentals2[3]} ---> Rental_ID:  {active_rentals2[4]} ---> Copy_ID: {active_rentals2[0]} \n");
                            Console.WriteLine("Enter the 'Rental_ID'");
                            int rental_id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Enter the 'Copy_ID'");
                            int copy_id = Convert.ToInt32(Console.ReadLine());
                            NpgsqlCommand rentals = new NpgsqlCommand($"UPDATE rentals SET return ='1' WHERE rental_id={rental_id} ", conn);
                            // Once we're done reading data we need to close the reader
                            active_rentals2.Close();
                            rentals.ExecuteNonQuery();
                            NpgsqlCommand copies = new NpgsqlCommand($"UPDATE copies SET available ='1' WHERE copy_id={copy_id} ", conn);
                            copies.ExecuteNonQuery();
                            Console.WriteLine("Return of a copy is regisgeristed");
                        }
                        break;

                    case "5":
                        {
                            Console.WriteLine("Please enter First Name of the User:");
                            string first_name = Console.ReadLine();
                            Console.WriteLine("Please enter Last Name of the User:");
                            string last_name = Console.ReadLine();
                            Console.WriteLine("Please enter Birthday of the User: (yyyy-mm-dd)");
                            string birthday = Console.ReadLine();



                            // Define a query
                            NpgsqlCommand cmd = new NpgsqlCommand($"insert into clients (first_name,last_name,birthday) values ('{first_name}','{last_name}','{birthday}')", conn);

                            cmd.ExecuteNonQuery();

                            Console.WriteLine("New user created");


                        }
                        break;
                    case "6":
                        {
                            var transaction = conn.BeginTransaction();

                            Console.WriteLine("Please enter Movie Name:");
                            string movie_name = Console.ReadLine();
                            Console.WriteLine("Please enter Movie Year:");
                            int year = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter Age Restriction: ");
                            int age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Please enter Price: ");
                            int price = Convert.ToInt32(Console.ReadLine());



                            // Define a query
                            NpgsqlCommand cmd = new NpgsqlCommand($"insert into movies (title,year,age_restriction,price) values ('{movie_name}',{year},{age},{price})", conn);
                            cmd.ExecuteNonQuery();
                            NpgsqlCommand active_rentals = new NpgsqlCommand($"SELECT movie_id from movies where title='{movie_name}'", conn);
                            NpgsqlDataReader active_rentals2 = active_rentals.ExecuteReader();
                            active_rentals2.Read();
                            Console.Write($"Name and Surname : {active_rentals2[0]} \n");
                            NpgsqlCommand copies = new NpgsqlCommand($"INSERT INTO copies (available, movie_id) VALUES (true,{active_rentals2[0]})", conn);
                            active_rentals2.Close();
                            copies.ExecuteNonQuery();

                            // Once we're done reading data we need to close the reader

                            transaction.Commit();





                            conn.Close();


                        }

                        break;

                    case "7":
                        {


                            NpgsqlCommand cmd = new NpgsqlCommand("SELECT rentals.*,clients.* FROM rentals,clients where return = '0' and date_of_return + interval '14' < current_date and rentals.client_id = clients.client_id", conn);
                            NpgsqlDataReader dataReader = cmd.ExecuteReader();
                            while (dataReader.Read())
                                Console.Write($"Name: {dataReader[7]} --> {dataReader[8]} --> Date of Rental: {dataReader[2]} --> Date of Return: {dataReader[3]} \n");
                            // Once we're done reading data we need to close the reader
                            dataReader.Close();

                        }
                        break;
                    case "0":
                        conn.Close();
                        System.Environment.Exit(1);
                        break;
                }
            }

        }
    }
}