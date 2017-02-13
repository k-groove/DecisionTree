using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DecisionTree
{
    class Program
    {
        static Node _rootNode = new Node("Root", string.Empty);
        static int bfsCount = 0;
        static int dfsCount = 0;
        static void Main(string[] args)
        {
            Console.WriteLine(" Enter xml file path: ");
            string filepath = Console.ReadLine();
            XmlDocument file = new XmlDocument();
            file.Load(filepath);
            XmlToTree(file);
            Start();
        }

        public static void Start()
        {
            //GetChoices(node);
            Console.Write(" Enter search term or quit to exit: ");
            var choice = Console.ReadLine();

            if (choice == "quit" || choice == "exit") Environment.Exit(0);

            BreadthFirstSearch(_rootNode, choice);//node, search term
            DepthFirstSearch(_rootNode, choice);//node, search term, counter
            bfsCount = 0;
            dfsCount = 0;
            Start();
        }

        public static void XmlToTree(XmlDocument file)
        {
            var rootElement = file.DocumentElement;

            foreach (XmlNode n in rootElement)
            {
                CreateTree(_rootNode, n);
            }
        }

        public static void CreateTree(Node parentNode, XmlNode node)
        {
            Node newNode = new Node(node.Attributes["behavior"]?.InnerText, node.Attributes["response"]?.InnerText);
            parentNode.children.Add(newNode);
            foreach (XmlNode n in node)
            {
                CreateTree(newNode, n);
            }
        }

        public static void BreadthFirstSearch(Node node, string input)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                node = queue.Dequeue();

                foreach (Node n in node.children)
                {
                    if (node != null)
                    {
                        queue.Enqueue(n);
                    }
                    if (n.behavior.ToLower() == input.ToLower())
                    {
                        Console.WriteLine(string.Format("{0} - found in {1} steps with breadth first search.", input, bfsCount));

                        if (!string.IsNullOrEmpty(n.children[0].response) && n.children.Count > 0)
                        {
                            var randomNum = GetRandom(n.children.Count);
                            Console.WriteLine("Response is: " + n.children[randomNum].response);
                            return;
                        }
                        BreadthFirstSearch(n, n.children[GetRandom(n.children.Count)].behavior);
                    }
                    bfsCount++;
                }
            }
            Console.WriteLine(string.Format("{0} was not found in the tree.", input));
        }

        public static void DepthFirstSearch(Node node, string input)
        {
            if (node == null)
            {
                return;
            }
            foreach (Node n in node.children)
            {
                if (n.behavior.ToLower() == input.ToLower())
                {
                    Console.WriteLine(string.Format("{0} - found in {1} steps with depth first search.", input, dfsCount));
                    if (!string.IsNullOrEmpty(n.children[0].response) && n.children.Count > 0)
                    {
                        var randomNum = GetRandom(n.children.Count);
                        Console.WriteLine("Response is: " + n.children[randomNum].response);
                        return;
                    }
                    DepthFirstSearch(n, n.children[GetRandom(n.children.Count)].behavior);
                    dfsCount++;
                }
                DepthFirstSearch(n, input);
                dfsCount++;
            }
        }

        //Return random number
        public static int GetRandom(int count)
        {
            Random rand = new Random();
            return rand.Next(count);
        }
    }

    public class Node
    {
        public string behavior;
        public string response;
        public List<Node> children = new List<Node>();

        //Constructor
        public Node(string behavior, string response)
        {
            this.behavior = behavior;
            this.response = response;
        }
    }
}
