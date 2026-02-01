using Quiz;


// Powołanie do życia obiektu game - który będzie zarządzał przebiegiem gry
var game = new Game();

// Wyświetlenie komunikatu powitalnego
game.DisplayWelcomeMessage();

// Zorganizowanie wszystkich pytań w grze przenoszę do konstruktora klasy Game


// TUTAJ
while (true)
{
    // Losowanie pytania z bieżącej kategorii
    game.DrawQuestionFromCurrentCategory();

    // Wyświetlanie aktualnego pytania graczowi
    game.CurrentQuestion.Display();

    // Pobranie numeru odpowiedzi od gracza
    var userNumber = Console.ReadLine();

    // Musimy zmusić gracza do wpisania 1, 2, 3 lub 4
    List<string> acceptedKeys = ["1", "2", "3", "4", "0"];

    if (game.isFiftyFiftyUsed == true)
    {
        acceptedKeys = ["1", "2", "3", "4"];
    }

    while (!acceptedKeys.Contains(userNumber))
    {
        Console.Clear();
        game.CurrentQuestion.Display();
        userNumber = Console.ReadLine();
    }

    while (userNumber == "0" && game.isFiftyFiftyUsed == false)
    {
        game.FiftyFiftyQuestion();
        acceptedKeys = ["1", "2"];
        userNumber = Console.ReadLine();
        while (!acceptedKeys.Contains(userNumber))
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Koło 50/50 zostało użyte");
            Console.ForegroundColor = ConsoleColor.White;
            game.CurrentQuestion.Display();
            userNumber = Console.ReadLine();
        }
        game.isFiftyFiftyUsed = true;
    }

   
        // Sprawdzanie odpowiedzi gracza
        var isCorrect = game.CheckUserAnswer(userNumber);

        Console.Clear();

        if (isCorrect)
        {
            // Dobra odpowiedź
            game.GoodAnswer();
            // Sprawdzenie czy to było ostatnie pytanie
            if (game.CurrentQuestion.Category == game.MaxCategory)
            {
                // Ostatnie pytanie
                // Gratulacje, ukończyłes/aś cały Quiz
                game.Success();
                break;
            }
            else
            {
                // Podniesienie kategorii na następną
                game.IncreaseCategory();
            }
        }
        else
        {
            // Zła odpowiedź
            game.FailGameOver();
            break;
            // KONIEC PROGRAMU
        }
    }




Console.ReadLine();


