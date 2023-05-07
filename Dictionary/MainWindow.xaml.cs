using RedBlackTreeImplementation;

using System.Collections.Generic;
using System.Windows;
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

        private void LoadDictionary()
        {
            // Load dictionary file into red-black tree
            // In this example, we're just adding a few words for demonstration purposes
            _dictionary.Insert("apple", "a fruit with a red or green skin and a white inside");
            _dictionary.Insert("banana", "a long curved fruit with a yellow skin");
            _dictionary.Insert("orange", "a round fruit with a thick orange skin");
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
            // Clear any previous results
            lstDefinitions.Items.Clear();

            // Get a random word from red-black tree
            KeyValuePair<string, string> randomWord = _dictionary.GetRandomNode();

            // Display word and definition
            lstDefinitions.Items.Add(randomWord.Key + ": " + randomWord.Value);
        }
    }
}