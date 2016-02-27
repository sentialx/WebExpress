namespace WebExpress
{

    public class HistItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }

    public class BookItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
    public class Values
    {
        //General settings

        public string SE { get; set; }
        public string Start { get; set; }

        //News settings

        public string Information { get; set; }
        public string Entertainment { get; set; }
        public string Moto { get; set; }
        public string Business { get; set; }
        public string Sport { get; set; }

        //Weather settings

        public string City { get; set; }

    }
}
