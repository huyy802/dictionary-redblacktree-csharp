using RedBlackTreeImplementation;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace RedBlackTreeDemo
{
    public partial class MainWindow : Window
    {
        private readonly RedBlackTree<string, string> _dictionary = new RedBlackTree<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            // Load dictionary file into red-black tree
            LoadDictionary();
        }

        private List<string> GetSuggestedWords(string searchTerm)
        {
            // Logic to fetch and return suggested words based on the search term
            List<string> suggestedWords = new List<string>();

            foreach (KeyValuePair<string, string> pair in _dictionary)
            {
                if (pair.Key.StartsWith(searchTerm))
                {
                    suggestedWords.Add(pair.Key);
                }
            }

            return suggestedWords;
        }

        private Popup suggestionPopup;

        private void ShowSuggestedWords(List<string> suggestedWords)
        {
            // Create or update the suggestion popup
            if (suggestionPopup == null)
            {
                suggestionPopup = new Popup();
                suggestionPopup.PlacementTarget = txtSearch;
                suggestionPopup.Placement = PlacementMode.Bottom;
                suggestionPopup.StaysOpen = false;
            }
            else
            {
                // If suggestionPopup already has a child, remove it
                if (suggestionPopup.Child is UIElement child)
                {
                    suggestionPopup.Child = null;
                }
            }

            // Create a new ListBox control to serve as the child of suggestionPopup
            ListBox listBox = new ListBox();
            foreach (var item in suggestedWords)
            {
                listBox.Items.Add(item);
            }

            // Assign the new ListBox as the child of suggestionPopup
            suggestionPopup.Child = listBox;

            // Open or close the suggestion popup based on the number of suggestions
            suggestionPopup.IsOpen = (suggestedWords.Count > 0);

            // Handle ListBox selection event
            listBox.SelectionChanged += (sender, e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    // Apply selected suggestion to the TextBox
                    txtSearch.Text = listBox.SelectedItem.ToString();

                    // Close the suggestion popup
                    suggestionPopup.IsOpen = false;
                }
            };
        }


        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();

            // Check if the search term is empty
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Close the suggestion popup
                suggestionPopup.IsOpen = false;
                return;
            }

            List<string> suggestedWords = GetSuggestedWords(searchTerm);

            // Display the suggested words in a separate window or control
            ShowSuggestedWords(suggestedWords);
        }

        private void LoadDictionary()
        {
            _dictionary.Insert("apple", "a fruit with a red or green skin and a white inside");
            _dictionary.Insert("banana", "a long curved fruit with a yellow skin");
            _dictionary.Insert("orange", "a round fruit with a thick orange skin");
            _dictionary.Insert("strawberry", "a small, sweet fruit with a red color");
            _dictionary.Insert("grape", "a small, round fruit that grows in clusters");
            _dictionary.Insert("watermelon", "a large, juicy fruit with a green rind and red flesh");
            _dictionary.Insert("pineapple", "a tropical fruit with a tough, spiky skin and sweet yellow flesh");
            _dictionary.Insert("mango", "a juicy fruit with a sweet, tropical flavor and orange flesh");
            _dictionary.Insert("pear", "a sweet fruit with a thin, smooth skin and juicy flesh");
            _dictionary.Insert("kiwi", "a small fruit with a brown, fuzzy skin and green flesh");
            _dictionary.Insert("cherry", "a small, round fruit that can be red, yellow, or black");
            _dictionary.Insert("blueberry", "a small, round fruit that is typically dark blue in color");
            _dictionary.Insert("lemon", "a yellow citrus fruit with a sour taste");
            _dictionary.Insert("lime", "a green citrus fruit with a sour taste");
            _dictionary.Insert("peach", "a juicy fruit with a fuzzy skin and sweet flesh");
            _dictionary.Insert("apricot", "a small, orange fruit with a soft flesh");
            _dictionary.Insert("plum", "a small, round fruit with a smooth skin and sweet flesh");
            _dictionary.Insert("nectarine", "a smooth-skinned fruit similar to a peach, but with a firmer flesh");
            _dictionary.Insert("raspberry", "a small, sweet fruit that grows on thorny bushes");
            _dictionary.Insert("blackberry", "a small, dark fruit that grows on thorny bushes");
            _dictionary.Insert("cranberry", "a small, sour fruit that is often used in sauces and juices");
            _dictionary.Insert("pomegranate", "a round fruit with a tough, leathery skin and juicy seeds");
            _dictionary.Insert("fig", "a sweet fruit with a soft flesh and a wrinkled skin");
            _dictionary.Insert("date", "a sweet fruit that grows on palm trees");
            _dictionary.Insert("coconut", "a large fruit with a hard, hairy shell and sweet, white flesh");
            _dictionary.Insert("avocado", "a fruit with a green, bumpy skin and creamy flesh");
            _dictionary.Insert("banana", "a long curved fruit with a yellow skin");
            _dictionary.Insert("guava", "a tropical fruit with a pink or green skin and sweet, juicy flesh");
            _dictionary.Insert("passion fruit", "a round or oval fruit with a hard, wrinkled skin and juicy seeds");
            _dictionary.Insert("dragon fruit", "a vibrant fruit with a scaly, bright pink or yellow skin and white or pink flesh");
            _dictionary.Insert("jackfruit", "a large, tropical fruit with a spiky green skin and sweet, yellow flesh");
            _dictionary.Insert("kiwifruit", "a small, oval fruit with a fuzzy brown skin and green flesh");
            _dictionary.Insert("lychee", "a small, round fruit with a rough, reddish-brown skin and sweet, white flesh");
            _dictionary.Insert("mangosteen", "a tropical fruit with a thick, purple rind and juicy, white flesh");
            _dictionary.Insert("papaya", "a large fruit with a green or yellow skin and orange flesh");
            _dictionary.Insert("persimmon", "a sweet fruit with a smooth, orange or reddish-brown skin");
        }

        private void OnSearchClicked(object sender, RoutedEventArgs e)
        {
            // Clear any previous results
            lstDefinitions.Items.Clear();

            // Get search term
            string searchTerm = txtSearch.Text.ToLower();

            // Search for word in red-black tree
            if (_dictionary.ContainsKey(searchTerm))
            {
                // Display definition of word
                lstDefinitions.Items.Add(_dictionary[searchTerm]);
            }
            else
            {
                // Display error message if word not found
                lstDefinitions.Items.Add("Word not found in dictionary");
            }
        }

        private void OnRandomWordClicked(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();

            // Clear any previous results
            lstDefinitions.Items.Clear();

            // Get a random word from red-black tree
            KeyValuePair<string, string> randomWord = _dictionary.GetRandomNode();

            // Display word and definition
            lstDefinitions.Items.Add(randomWord.Key + ": " + randomWord.Value);
        }
    }
}