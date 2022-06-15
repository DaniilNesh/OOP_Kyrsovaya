namespace OOP_Kyrsovaya
{
    public class Medicines
    {
        public string Title { get; set; }
        public string Illness { get; set; }
        public double Price { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="title"></param>
        /// <param name="illness"></param>
        /// <param name="price"></param>
        public Medicines(string title, string illness, double price)
        {
            Title = title;
            Illness = illness;
            Price = price;
        }
        /// <summary>
        /// Вывод класса
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Название: {Title} Болезнь:{Illness} Цена: {Price}";
        }

    }
}
