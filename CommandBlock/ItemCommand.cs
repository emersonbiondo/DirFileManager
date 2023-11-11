namespace CommandBlock
{
    public class ItemCommand
    {
        public int OrderId { get; set; }

        public string Type { get; set; }

        public int Delay { get; set; }

        public string Parameter1 { get; set; }

        public string Parameter2 { get; set; }

        public ItemCommand()
        {
            OrderId = 0;
            Type = string.Empty;
            Delay = 1;
            Parameter1 = string.Empty;
            Parameter2 = string.Empty;
        }
    }
}
