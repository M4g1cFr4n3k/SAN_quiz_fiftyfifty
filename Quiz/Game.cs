using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz;

public class Game
{
    public Game()
    {
        CreateQuestionDatabase();
        AllCategories = QuestionDatabase.Select(p => p.Category).OrderBy(c => c).Distinct().ToList();
        CurrentCategory = AllCategories[0];
        MaxCategory = AllCategories[AllCategories.Count - 1];

    }

    // Kategoria maxymalna
    public int MaxCategory { get; set; }

    // Aktualna kategoria pytań, z której losujemy pytania
    public int CurrentCategory { get; set; }

    // Baza wszystkich pytań w grze
    public List<Question> QuestionDatabase { get; set; }

    // Aktualne pytanie, na które gracz odpowiada
    public Question CurrentQuestion { get; set; }

    public List<int> AllCategories { get; set; }

    public int CurrentCategoryIndex { get; set; }

    private Random random = new Random();
    
    public bool isFiftyFiftyUsed = false;

    public bool isThisRoundFiftyFifty = false;

    public void DisplayWelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine(" Witaj w aplikacji Quiz");
        Console.WriteLine(" Spróbuj oddpowiedzieć na 7 pytań");
        Console.WriteLine(" Powodzenia !!!");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Naciśnij ENTER, aby zobaczyć pierwsze pytanie ... ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadLine();
        Console.Clear();
    }

    public void CreateQuestionDatabase()
    {
        var path = $"{Directory.GetCurrentDirectory()}/questions.json";
        var data = File.ReadAllText(path);
        QuestionDatabase = JsonConvert.DeserializeObject<List<Question>>(data);
    }
    
    public void DrawQuestionFromCurrentCategory()
    {
        var questionsFromCurrentCategory = new List<Question>();
        foreach (var question in QuestionDatabase)
        {
            if (question.Category == CurrentCategory)
                questionsFromCurrentCategory.Add(question);
        }

        var index = random.Next(0, questionsFromCurrentCategory.Count);
        var selectectedQuestion = questionsFromCurrentCategory[index];
        selectectedQuestion.Answers = selectectedQuestion.Answers.OrderBy(x => random.Next()).ToList();
        var counter = 1;
        foreach (var answer in selectectedQuestion.Answers)
        {
            answer.Order = counter;
            counter++;
        }


        CurrentQuestion = selectectedQuestion;
    }

    public void FiftyFiftyQuestion()
    {
        if (!isFiftyFiftyUsed)
        {
            int removedAnswers = 0;
            for (int i = CurrentQuestion.Answers.Count - 1; i >= 0; i--)
            {
                if (!CurrentQuestion.Answers[i].IsCorrect && removedAnswers < 2)
                {
                    CurrentQuestion.Answers.RemoveAt(i);
                    removedAnswers++;
                }
            }

            var counter = 1;
            foreach (var answer in CurrentQuestion.Answers)
            {
                answer.Order = counter;
                counter++;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Koło 50/50 zostało użyte");
            Console.ForegroundColor = ConsoleColor.White;
            CurrentQuestion.Display();
        }
        isThisRoundFiftyFifty = true;
    }

    public bool CheckUserAnswer(string userNumber)
    {
        // logika w której na podstawie numeru gracza, ustalamy czy odpowiedź jest poprawna lub nie
        foreach (var answer in CurrentQuestion.Answers)
        {
            // musimy zamienić odpowiedź gracza na integer
            var userNumberAsInteger = int.Parse(userNumber);
            // nas interesuje ta sytuacja:
            if (userNumberAsInteger == answer.Order)
            {
                return answer.IsCorrect;
            }
        }

        return false;

        //return CurrentQuestion.Answers.First(x => x.Id == int.Parse(userNumber)).IsCorrect;
    }

    public void GoodAnswer()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Brawo, to jest poprawna odpowiedź. Wygrałeś/aś {CurrentQuestion.Category} pkt");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Naciśnij ENTER, aby zobaczyć następne pytanie ...");
        Console.ReadLine();
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void FailGameOver()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" Ups, to niesety nie jest prawidłowa opowiedź ...");
        Console.WriteLine(" KONIEC GRY");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void Success()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Brawo, to jest poprawna odpowiedź. Wygrałeś/aś {CurrentQuestion.Category} pkt");
        Console.Write(" Gratulacje, ukończyłeś/aś cały Quiz !!!");
        Console.Write(" KONIEC GRY");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void IncreaseCategory()
    {
        CurrentCategoryIndex++;
        CurrentCategory = AllCategories[CurrentCategoryIndex];
        isThisRoundFiftyFifty = false;
    }
}
