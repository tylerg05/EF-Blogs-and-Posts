using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                var db = new BloggingContext();
                var userInput = "1";
                while (userInput.Equals("1") || userInput.Equals("2") || userInput.Equals("3") || userInput.Equals("4"))
                {
                    Console.WriteLine("1. Display all blogs");
                    Console.WriteLine("2. Add blog");
                    Console.WriteLine("3. Create post");
                    Console.WriteLine("4. Display all posts");
                    Console.WriteLine("Enter any other character to quit");
                    userInput = Console.ReadLine();
                    if (userInput == "1")
                    {
                        // Show all blogs
                        var blogs = db.Blogs;
                        var blogsAmount = blogs.Count();
                        Console.WriteLine(blogsAmount + " blogs found");
                        foreach (var item in blogs)
                        {
                            Console.WriteLine(item.Name);
                        }

                    }
                    else if (userInput == "2")
                    {
                        // Ask user for new Blog name
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        if (name.Equals(""))
                        {
                            Console.WriteLine("Blog name cannot be null");
                        }
                        else
                        {
                            // save blog
                            var newBlog = new Blog { Name = name };
                            db.Blogs.Add(newBlog);
                            db.SaveChanges();
                            //logger.Info("new blog saved");
                        }
                    }
                    else if (userInput == "3")
                    {
                        // Show all blogs
                        var blogs = db.Blogs;
                        Console.WriteLine("Here are all the blogs:");
                        foreach (var item in blogs)
                        {
                            Console.WriteLine(item.BlogId + ") " + item.Name);
                        }

                        // Ask user to select blog
                        Console.Write("Enter a Blog ID: ");
                        var userChoice = Int32.Parse(Console.ReadLine());
                        var validBlogIDFlag = false;
                        foreach (var item in blogs)
                        {
                            if (item.BlogId == userChoice)
                            {
                                validBlogIDFlag = true;
                            }
                        }
                        
                        if (validBlogIDFlag)
                        {
                            var blog = blogs.FirstOrDefault(b => b.BlogId == userChoice);


                            // Ask user to enter post details
                            Console.WriteLine("Enter a Post");

                            Console.WriteLine("Enter a title");
                            var title = Console.ReadLine();
                            if (title.Equals(""))
                            {
                                Console.WriteLine("Post title cannot be null");
                            }
                            else
                            {
                                Console.WriteLine("Enter some content");
                                var content = Console.ReadLine();

                                // save the post
                                var newPost = new Post
                                {
                                    Title = title,
                                    Content = content,
                                    BlogId = blog.BlogId
                                };

                                db.Posts.Add(newPost);
                                db.SaveChanges();
                                //logger.Info("new post saved");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Blog ID");
                        }
                    }
                    else if (userInput == "4")
                    {
                        // Show all blogs
                        var blogs = db.Blogs;
                        Console.WriteLine("Here are all the blogs:");
                        Console.WriteLine("0) Posts from all blogs");
                        foreach (var item in blogs)
                        {
                            Console.WriteLine(item.BlogId + ") " + item.Name);
                        }

                        // Ask user to select blog
                        Console.Write("Enter a Blog ID: ");
                        var userChoice = Int32.Parse(Console.ReadLine());
                        if (userChoice == 0)
                        {
                            foreach (var item in db.Posts)
                            {
                                Console.WriteLine("Blog: " + item.Blog);
                                Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                            }
                        }
                        else
                        {
                            var blog = blogs.FirstOrDefault(b => b.BlogId == userChoice);

                            // display the posts
                            Console.WriteLine("Here are the posts:");
                            foreach (var item in blog.Posts)
                            {
                                Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("The program will now close");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
