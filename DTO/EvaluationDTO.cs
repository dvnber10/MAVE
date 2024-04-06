using Castle.Core;

namespace MAVE.DTO
{
    public class EvaluationDTO
    {
        //Numero de pregunta
        public int Question {  get; set; }
        //Opcion elegida por el usuario
        public char Option { get; set; } 
    }
}
