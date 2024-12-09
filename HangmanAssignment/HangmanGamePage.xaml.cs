using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private string _currentWord;
        private char[] _displayedWord;
        private int _incorrectGuesses;
        private const int MaxGuesses = 5;
        private List<string> _hangmanImages = new List<string>
        {
            "hang1.png", "hang2.png", "hang3.png",
            "hang4.png", "hang5.png", "hang6.png",
            "hang7.png", "hang8.png"
        };

        // List of words to guess
        private List<string> _wordList = new List<string>
        {
            "SAMSUNG", "PROGRAMMING", "HANGMAN", "COMPUTER",
            "SOFTWARE", "DEVELOPMENT", "MOKGAETJI", "MASHABELA", "LDIL",
        };

        // List to keep track of guessed letters
        private HashSet<char> _guessedLetters = new HashSet<char>();

        public HangmanGamePage()
        {
            InitializeComponent();
            InitializeGame();
        }

        // Game Initialization
        private void InitializeGame()
        {
            var random = new Random();
            _currentWord = _wordList[random.Next(_wordList.Count)].ToUpper(); // Select a random word

            _displayedWord = new char[_currentWord.Length];
            for (int i = 0; i < _currentWord.Length; i++) // Word Increment
            {
                _displayedWord[i] = '_';
            }

            // Provide a hint by revealing one random letter in the word
            int hintIndex = random.Next(_currentWord.Length);
            _displayedWord[hintIndex] = _currentWord[hintIndex];
            _guessedLetters.Add(_currentWord[hintIndex]); // Add the hint letter to the guessed list

            WordToGuessLabel.Text = new string(_displayedWord);
            _incorrectGuesses = 0;
            HangmanImage.Source = _hangmanImages[_incorrectGuesses];
            ResultLabel.Text = string.Empty;
            _guessedLetters.Clear(); // Clear the guessed letters list
        }

        // Handling the guess click event
        private void OnGuessClicked(object sender, EventArgs e)
        {
            string guess = GuessEntry.Text?.ToUpper();
            if (string.IsNullOrWhiteSpace(guess) || guess.Length != 1)
            {
                ResultLabel.Text = "Please enter a single letter.";
                return;
            }

            char guessedLetter = guess[0];

            if (_guessedLetters.Contains(guessedLetter))
            {
                ResultLabel.Text = $"The letter '{guessedLetter}' has already been guessed. Try a different letter.";
                return;
            }

            _guessedLetters.Add(guessedLetter);
            bool isCorrectGuess = false;

            for (int i = 0; i < _currentWord.Length; i++)
            {
                if (_currentWord[i] == guessedLetter)
                {
                    _displayedWord[i] = guessedLetter;
                    isCorrectGuess = true;
                }
            }

            if (isCorrectGuess)
            {
                WordToGuessLabel.Text = new string(_displayedWord);
                ResultLabel.Text = string.Empty;
                if (!WordToGuessLabel.Text.Contains('_'))
                {
                    ResultLabel.Text = "Congratulations! You've won!";
                    PlayAgainButton.IsVisible = true; // Show the Play Again button
                }
            }
            else
            {
                //for incorrect guesses
                _incorrectGuesses++;
                HangmanImage.Source = _hangmanImages[_incorrectGuesses];
                ResultLabel.Text = "Incorrect guess!";
                if (_incorrectGuesses >= MaxGuesses)
                {
                    ResultLabel.Text = "Game Over! The word was " + _currentWord;
                    PlayAgainButton.IsVisible = true; // Show the Play Again button
                }
            }

            GuessEntry.Text = string.Empty;
        }

        // Handling the Play Again click event
        private void OnPlayAgainClicked(object sender, EventArgs e)
        {
            InitializeGame();
        }
    }
}
