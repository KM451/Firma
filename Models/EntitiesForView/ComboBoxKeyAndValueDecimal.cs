namespace Firma.Models.EntitiesForView
{
    public class ComboBoxKeyAndValueDecimal
    {
        #region Konstruktory
        public ComboBoxKeyAndValueDecimal() { }
        public ComboBoxKeyAndValueDecimal(decimal key, string value)
        {
            Key = key;
            Value = value;
        }
        #endregion
        /// <summary>
        /// Pole do przekazania do Modelu. Bedzie to przykładowo IdKontrahenta
        /// </summary>
        public decimal Key { get; set; }

        /// <summary>
        /// Wartość wyświetlana użytkownikowi na widoku
        /// </summary>
        public string Value { get; set; }
    }
}
