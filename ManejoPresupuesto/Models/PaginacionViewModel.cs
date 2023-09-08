namespace ManejoPresupuesto.Models
{
    public class PaginacionViewModel
    {
        public int Pagina { get; set; } = 1;

        private int recordsPorPagina = 10;

        private readonly int cantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get 
            {
                return recordsPorPagina; 
            }
            set
            {
                //value = 10
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina ) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
        public int RecordASaltar => recordsPorPagina * (Pagina - 1);
    }
}
