using System.Numerics;

namespace creditcardvalidator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool work = true;
            while (work)
            {
                try
                {
                    work = mainMenu();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong. Please try again.");
                    continue;
                }

            }
        }

        static bool luhnAlgorithmValidator(string cardNumber)
        {
            char[] cardNumberTab = cardNumber.ToCharArray();
            int controlSum = 0;
            bool isEven = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int number;
                if (isEven)
                {
                    number = Convert.ToInt32(cardNumber[i]) * 2;
                    if (number > 9)
                    {
                        number = 1 + (number % 10);
                    }
                }
                else { number = Convert.ToInt32(cardNumber[i]); }
                controlSum += number;
                isEven = !isEven;
            }

            return controlSum % 10 == 0;
        }

        static void changeColors(bool isPositive)
        {
            if (isPositive)
            {
                //Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                //Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }

        static bool lengthValidator(int cardNumberLength)
        {
            return cardNumberLength <= 19 && cardNumberLength >= 8 ? true : false;
        }

        static string issuerIdentificationNumberValidator(string cardNumber)
        {
            bool cardNumberLengthValid = lengthValidator(cardNumber.Length);
            List<string> americanExpressIins = new List<string> { "34", "37" };
            List<string> maestroIins = new List<string> { "5018", "5020", "5038", "5893", "6304", "6759", "6761", "6762", "6763" };
            if (cardNumberLengthValid)
            {
                string iinNumber = cardNumber.Substring(0, 4);
                if (((int.Parse(iinNumber) >= 2221 && int.Parse(iinNumber) <= 2720) ||
                    (int.Parse(iinNumber.Substring(0, 2)) >= 51 && int.Parse(iinNumber.Substring(0, 2)) <= 55)) && cardNumber.Length == 16) return "MasterCard";
                else if (americanExpressIins.Contains(iinNumber.Substring(0, 2)) && cardNumber.Length == 15) return "Amierican Express";
                else if (iinNumber.StartsWith("4") && (cardNumber.Length == 13 || cardNumber.Length == 16)) return "Visa";
                else if (maestroIins.Contains(iinNumber) && (cardNumber.Length >= 12 && cardNumber.Length <= 19)) return "Maestro";
                else return "Undefined Provider";
            }
            return "Invalid Card Number";
        }

        static bool mainMenu()
        {
            Console.WriteLine("Choose option:");
            string[] options = new string[]
            {
                "Validate credit card",
                "Generate credit card number",
                "Generate X credit cards' numbers and validate",
                "Quit"
            };
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, options[i]);
            }
            Console.Write("Option: ");
            int option = Convert.ToInt32(Console.ReadLine());

            switch (option - 1)
            {
                case 0:
                    Console.Write("Input card number: ");
                    string? cardNumber = Console.ReadLine();
                    if (cardNumber == null) break;
                    bool luhnIsValid = luhnAlgorithmValidator(cardNumber);
                    string iinProvider = issuerIdentificationNumberValidator(cardNumber);
                    changeColors(luhnIsValid && iinProvider != "Invalid Card Number");
                    Console.WriteLine(luhnIsValid && iinProvider != "Invalid Card Number" ? "Valid number - {0}" : "Invalid number - {0}", iinProvider);
                    Console.ResetColor();
                    break;
                case 1:
                    Console.WriteLine("Your Valid Card Number: {0}",generateCreditCardNumber());
                    break;
                case 2:
                    Console.WriteLine("How many numbers do you want to generate?: ");
                    int nbCreditCards = Convert.ToInt32(Console.ReadLine());
                    List<(string, bool)> cardNumbers = generateCreditCardNumber(nbCreditCards);
                    cardNumbers.ForEach(cardNumber =>
                    {
                        changeColors(cardNumber.Item2);
                        Console.WriteLine("{0} - {1}", cardNumber.Item1, cardNumber.Item2 ? "Valid" : "Invalid");
                        Console.ResetColor();
                    });
                    break;
                case 3:
                    return false;
                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
            return true;
        }

        private static string generateCreditCardNumber()
        {
            Random random = new Random();
            BigInteger counter = new BigInteger(1);
            string resultCardNumber;
            Console.WriteLine("Generating Credit Card Number - MasterCard | Amierican Express | Visa | Maestro");
            string iinNumber = "";
            while (true)
            {
                int randomLength = random.Next(12,19);
                string secondPartOfNumber = "";
                for (int i = 0; i <= randomLength-1; i++)
                {
                    secondPartOfNumber = String.Concat(secondPartOfNumber, random.Next(1, 9).ToString());
                }
                resultCardNumber = String.Concat(iinNumber, secondPartOfNumber);
                bool luhnIsValid = luhnAlgorithmValidator(resultCardNumber);
                string iinProvider = issuerIdentificationNumberValidator(resultCardNumber);
                bool isValid = luhnIsValid && iinProvider != "Invalid Card Number" && iinProvider != "Undefined Provider";
                changeColors(isValid);
                Console.WriteLine("{0} - {1} - {2}", resultCardNumber, isValid ? "Valid" : "Invalid", iinProvider);
                Console.Write("");
                if (isValid) {
                    Console.ResetColor();
                    Console.Write("");
                    Console.WriteLine("Created {0} credit card numbers", counter);
                    Console.WriteLine();
                    break;
                } 
                Console.ResetColor();
                counter++;
            }
            return resultCardNumber;
        }

        private static (string, bool) generateSingleCreditCard()
        {
            Random random = new Random();
            string resultCardNumber;
            string iinNumber = random.Next(2221, 2720).ToString();
            string secondPartOfNumber = "";
            for (int i = 0; i <= 2; i++)
            {
                secondPartOfNumber = String.Concat(secondPartOfNumber, random.Next(1000, 9999).ToString());
            }
            resultCardNumber = String.Concat(iinNumber, secondPartOfNumber);
            bool isValid = luhnAlgorithmValidator(resultCardNumber);
            return (resultCardNumber, isValid);
        }

        private static List<(string, bool)> generateCreditCardNumber(int nbCreditCards)
        {
            List<(string, bool)> creditCards = new List<(string, bool)>();
            Console.WriteLine("Generating {0} Credit Card Numbers  - MasterCard", nbCreditCards.ToString());
            for (int i = 0; i < nbCreditCards; i++)
            {
                creditCards.Add(generateSingleCreditCard());
            }
            return creditCards;
        }
    }
}