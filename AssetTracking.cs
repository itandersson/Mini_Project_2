﻿using Mini_Project_2.Model;
using Mini_Project_2.Asset;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mini_Project_2
{
    internal class AssetTracking
    {
        static AssetsContext db = new AssetsContext();
        static List<Assets> assetList = new List<Assets>();

    static void Main(string[] args)
        {
            Assets asset = new Assets();

            // Read from database
            var assets = db.Asset
            .OrderBy(item => item.Id)
            .ToList();

            // Add to list
            assetList.AddRange(assets);

            // Note: Only for test, To activate predefined items
            //assetList.Add(new Iphone(DateTime.Parse("2018/12/29"), 970, "Phone", "iPhone", "8", "Spain", "EUR"));
            //assetList.Add(new HP(DateTime.Parse("2019/06/01"), 1423, "Computer", "HP", "Elitebook", "Spain", "EUR"));
            //assetList.Add(new Iphone(DateTime.Parse("2020/09/25"), 990, "Phone", "iPhone", "11", "Spain", "EUR"));
            //assetList.Add(new Iphone(DateTime.Parse("2018/07/15"), 1245, "Phone", "iPhone", "X", "Sweden", "SEK"));
            //assetList.Add(new Motorola(DateTime.Parse("2020/03/16"), 970, "Phone", "Motorola", "Razr", "Sweden", "SEK"));
            //assetList.Add(new HP(DateTime.Parse("2020/10/02"), 588, "Computer", "HP", "Elitebook", "Sweden", "SEK"));
            //assetList.Add(new Asus(DateTime.Parse("2017/04/21"), 1200, "Computer", "Asus", "W234", "USA", "USD"));
            //assetList.Add(new Lenovo(DateTime.Parse("2018/05/28"), 835, "Computer", "Lenovo", "Yoga 730", "USA", "USD"));
            //assetList.Add(new Lenovo(DateTime.Parse("2019/05/21"), 1030, "Computer", "Lenovo", "Yoga 530", "USA", "USD"));
            //db.AddRange(assetList);
            //db.SaveChanges();

            //Prints a sorted list with computers first, then phones
            List<Assets> SortedCPuterFirst = assetList.OrderBy(item => item.type).ToList<Assets>();
            asset.setList(SortedCPuterFirst);
            asset.setTempNumber();
            asset.printSortedList();

            input(asset);
        }

        /// <summary>
        /// Manages user interface
        /// </summary>
        /// <param name="asset"></param>
        private static void input(Assets asset)
        {
            bool run = true;

            string welcome = "Exit program by write \"q\" (for quit).!\n\n" +
                "(1) Show Assets (Sorted by office)\n" +
                "(2) Show Assets (Sorted by purchase date)\n" +
                "(3) Update Office (The asset has been sent to a new office)\n" +
                "(4) Add New Asset\n" +
                "(5) Delete Asset\n";

            while (run)
            {
                Console.WriteLine(welcome);
                Console.Write("Pick an option: ");
                string input = Console.ReadLine();

                //If "q" chosen quit
                if (input.Trim().ToLower() == "q") { run = false; }

                int value;
                int.TryParse(input, out value);

                try
                {
                    switch (value)
                    {
                        case 1:
                            //Prints a sorted list by office
                            List<Assets> SortedByOffice = assetList.OrderBy(item => item.office).ToList<Assets>();
                            asset.setList(SortedByOffice);
                            asset.setTempNumber();
                            asset.printSortedList();
                            continue;
                        case 2:
                            //Prints a sorted list by purchase date
                            List<Assets> SortedByPurchase = assetList.OrderBy(item => item.purchaseDate).ToList<Assets>();
                            asset.setList(SortedByPurchase);
                            asset.setTempNumber();
                            asset.printSortedList();
                            continue;
                        case 3:
                            updateOffice();
                            continue;
                        case 4:
                            addNewAsset();
                            continue;
                        case 5:
                            deleteAsset();
                            continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine('\t' + "Error: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Creates a new asset if no error
        /// </summary>
        private static void addNewAsset()
        {
            Console.Write("\tEnter purchase date (Ex: 2022/05/09): ");
            string purchaseDate = Console.ReadLine();

            Console.Write("\tEnter Price (USD): ");
            int price;
            int.TryParse(Console.ReadLine(), out price);

            Console.Write("\tEnter type (Phone or Computer): ");
            string type = Console.ReadLine();

            Console.Write("\tEnter brand (Ex: IPhone, Asus and Motorola): ");
            string brand = Console.ReadLine();

            Console.Write("\tEnter model (Ex: 8, W234 and Razr): ");
            string model = Console.ReadLine();

            Console.Write("\tEnter office (Spain, Sweden or USA): ");
            string office = Console.ReadLine();

            Console.Write("\n");

            Assets asset = createAsset(purchaseDate, price, type, brand, model, office);

            //If the asset was created
            if (asset != null)
            {
                assetList.Add(asset); // Add to list
                db.Add(asset); // Add to database
                db.SaveChanges();

                Console.WriteLine('\t' + "Thank you! your asset was successfully added" + '\n');
            }
            else { Console.WriteLine('\t' + "An error occurred! Please try again" + '\n'); }

        }

        /// <summary>
        /// Returns a new asset if no error occurred
        /// </summary>
        /// <param name="purchaseDate"></param>
        /// <param name="price"></param>
        /// <param name="type"></param>
        /// <param name="brand"></param>
        /// <param name="model"></param>
        /// <param name="office"></param>
        /// <returns></returns>
        private static Assets createAsset(string purchaseDate, int price, string type, string brand, string model, string office)
        {
            bool fail = false;
            Assets asset = null;
            string currency = "";
            type = "" + char.ToUpper(type[0]) + type.Substring(1);

            brand = brand.ToLower();
            office = office.ToLower();

            //Return a error if exceptions occurred
            try
            {

                //Assign currency by office
                switch (office)
                {
                case "spain":
                    currency = "EUR";
                    office = "" + char.ToUpper(office[0]) + office.Substring(1);
                    break;

                case "sweden":
                    currency = "SEK";
                    office = "" + char.ToUpper(office[0]) + office.Substring(1);
                    break;

                case "usa":
                    currency = "USD";
                    office = office.ToUpper();
                    break;

                default:
                    fail = true;
                    Console.WriteLine('\t' + "Error: \"" + office + "\" does not exist!");
                    break;
                }

                //Create asset if brand match
                switch (brand)
                {
                    case "macbook":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new MacBook(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "asus":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new Asus(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "lenovo":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new Lenovo(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "hp":
                        brand = brand.ToUpper();
                        asset = new HP(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "iphone":
                        brand = "" + brand[0] + char.ToUpper(brand[1]) + brand.Substring(2);
                        asset = new Iphone(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "samsung":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new Samsung(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "nokia":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new Nokia(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    case "motorola":
                        brand = "" + char.ToUpper(brand[0]) + brand.Substring(1);
                        asset = new Motorola(DateTime.Parse(purchaseDate), price, type, brand, model, office, currency);
                        break;

                    default:
                        fail = true;
                        Console.WriteLine('\t' + "Error: \"" + brand + "\" is not a allowed brand!");
                        break;
                }
            }
            catch (Exception e) 
            {
                fail = true;
                Console.WriteLine('\t' + "Error: " + e.Message );
            }

            //if an error occurred, ensure that null is returned
            if (fail) { asset = null; }

            return asset;
        }

        /// <summary>
        /// Updates the office property in one asset
        /// </summary>
        private static void updateOffice()
        {
            Assets asset = new Assets();

            //Prints a sorted list by purchase date
            List<Assets> SortedByPurchase = assetList.OrderBy(item => item.purchaseDate).ToList<Assets>();
            asset.setList(SortedByPurchase);
            asset.setTempNumber();
            asset.printSortedList();

            //Ask the user for nr
            string delete = "What asset do you want to update?";
            Console.WriteLine(delete);
            Console.Write("Select a number: ");
            int number = int.Parse(Console.ReadLine());

            //Get the asset
            int id = SortedByPurchase[--number].Id; //Find assets id
            Assets updateAsset = SortedByPurchase.First(item => item.Id == id);

            //Ask the user for new office
            string update = "Which office should the asset belong to?";
            Console.WriteLine(update);
            Console.Write("Enter office (Spain, Sweden or USA): ");
            string office = Console.ReadLine();

            //Update List and the database
            int index = assetList.IndexOf(updateAsset);
            updateAsset.office = office;
            assetList.RemoveAt(index);
            assetList.Insert(index, updateAsset);
            db.Update(updateAsset);
            db.SaveChanges();
        }

        /// <summary>
        /// Removes an asset from the List and the database
        /// </summary>
        private static void deleteAsset()
        {
            Assets asset = new Assets();

            //Prints a sorted list by purchase date
            List<Assets> SortedByPurchase = assetList.OrderBy(item => item.purchaseDate).ToList<Assets>();
            asset.setList(SortedByPurchase);
            asset.setTempNumber();
            asset.printSortedList();

            //Ask the user for Nr
            string delete = "What asset do you want to delete?";
            Console.WriteLine(delete);
            Console.Write("Select a number: ");
            int number = int.Parse(Console.ReadLine());

            //Try to delete asset, First from List and then from the database
            try
            {
                int id = SortedByPurchase[--number].Id; //Find assets id
                Assets deleteAsset = SortedByPurchase.First(item => item.Id == id);
                db.Remove(deleteAsset);
                db.SaveChanges();
                assetList.Remove(deleteAsset);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError: " + e.Message + "\n");
            }
        }
    }
}
