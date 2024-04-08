using MAVE.DTO;
using Org.BouncyCastle.Crypto.Digests;

namespace MAVE.Utilities
{
    public class EvaluationUtility
    {
        private List<char> Answers;
        public List<char> GetAnswers() 
        {
            return Answers;
        }
        public void SetAnswers(List<char> answers)
        {
            this.Answers = answers;
        }

        public short Score()
        {
            try
            {
                //Contadores para saber la cantidad de veces que el usuario eligió un perfil 
                int dScore = 0, iScore = 0, sScore = 0, cScore = 0, i = 1;
                List<char> results = GetAnswers();
                //Recorre todas las respuestas y va asignando los valores de cada pregunta a su respectivo contador
                foreach (char answer in results)
                {
                    switch (i)
                    {
                        case 1:
                            switch (answer)
                            {
                                case 'A':
                                    sScore++;
                                    break;
                                case 'B':
                                    iScore++;
                                    break;
                                case 'C':
                                    cScore++;
                                    break;
                                case 'D':
                                    break;
                            }
                            break;
                        case 2:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':
                                    cScore++;
                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':
                                    break;
                            }
                            break;
                        case 3:
                            switch (answer)
                            {
                                case 'A':
                                    break;
                                case 'B':
                                    dScore++;
                                    break;
                                case 'C':
                                    sScore++;
                                    break;
                                case 'D':
                                    iScore++;
                                    break;
                            }
                            break;
                        case 4:
                            switch (answer)
                            {
                                case 'A':
                                    cScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    iScore++;
                                    break;
                            }
                            break;
                        case 5:
                            switch (answer)
                            {
                                case 'A':

                                    break;
                                case 'B':
                                    cScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 6:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':

                                    break;
                            }
                            break;
                        case 7:
                            switch (answer)
                            {
                                case 'A':

                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':
                                    iScore++;
                                    break;
                            }
                            break;
                        case 8:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    iScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':

                                    break;
                            }
                            break;
                        case 9:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':
                                    cScore++;
                                    break;
                            }
                            break;
                        case 10:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    cScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 11:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    dScore++;
                                    break;
                            }
                            break;
                        case 12:
                            switch (answer)
                            {
                                case 'A':

                                    break;
                                case 'B':
                                    dScore++;
                                    break;
                                case 'C':
                                    cScore++;
                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 13:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    iScore++;
                                    break;
                                case 'C':
                                    sScore++;
                                    break;
                                case 'D':

                                    break;
                            }
                            break;
                        case 14:
                            switch (answer)
                            {
                                case 'A':
                                    cScore++;
                                    break;
                                case 'B':
                                    dScore++;
                                    break;
                                case 'C':
                                    iScore++;
                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 15:
                            switch (answer)
                            {
                                case 'A':
                                    sScore++;
                                    break;
                                case 'B':

                                    break;
                                case 'C':
                                    cScore++;
                                    break;
                                case 'D':

                                    break;
                            }
                            break;
                        case 16:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':

                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    dScore++;
                                    break;
                            }
                            break;
                        case 17:
                            switch (answer)
                            {
                                case 'A':
                                    cScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    dScore++;
                                    break;
                            }
                            break;
                        case 18:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    dScore++;
                                    break;
                            }
                            break;
                        case 19:
                            switch (answer)
                            {
                                case 'A':
                                    cScore++;
                                    break;
                                case 'B':
                                    dScore++;
                                    break;
                                case 'C':
                                    iScore++;
                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 20:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    cScore++;
                                    break;
                                case 'C':

                                    break;
                                case 'D':
                                    iScore++;
                                    break;
                            }
                            break;
                        case 21:
                            switch (answer)
                            {
                                case 'A':
                                    sScore++;
                                    break;
                                case 'B':

                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':
                                    cScore++;
                                    break;
                            }
                            break;
                        case 22:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':

                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':
                                    sScore++;
                                    break;
                            }
                            break;
                        case 23:
                            switch (answer)
                            {
                                case 'A':
                                    iScore++;
                                    break;
                                case 'B':
                                    cScore++;
                                    break;
                                case 'C':
                                    dScore++;
                                    break;
                                case 'D':

                                    break;
                            }
                            break;
                        case 24:
                            switch (answer)
                            {
                                case 'A':
                                    dScore++;
                                    break;
                                case 'B':
                                    sScore++;
                                    break;
                                case 'C':
                                    iScore++;
                                    break;
                                case 'D':
                                    cScore++;
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                //Segun la cantidad de veces seleccionado se le asigna un valor a cada contador
                switch (dScore)
                {
                    case 0:
                        dScore = 5;
                        break;
                    case 1:
                        dScore = 15;
                        break;
                    case 2:
                        dScore = 24;
                        break;
                    case 3:
                        dScore = 34;
                        break;
                    case 4:
                        dScore = 38;
                        break;
                    case 5:
                        dScore = 43;
                        break;
                    case 6:
                        dScore = 48;
                        break;
                    case 7:
                        dScore = 54;
                        break;
                    case 8:
                        dScore = 59;
                        break;
                    case 9:
                        dScore = 65;
                        break;
                    case 10:
                        dScore = 74;
                        break;
                    case 11:
                        dScore = 76;
                        break;
                    case 12:
                        dScore = 79;
                        break;
                    case 13:
                        dScore = 83;
                        break;
                    case 14:
                        dScore = 85;
                        break;
                    case 15:
                        dScore = 94;
                        break;
                    case 16:
                        dScore = 97;
                        break;
                    case 17:
                        dScore = 97;
                        break;
                    case 18:
                        dScore = 97;
                        break;
                    case 19:
                        dScore = 97;
                        break;
                    case 20:
                        dScore = 100;
                        break;
                    case 21:
                        dScore = 100;
                        break;
                }
                switch (iScore)
                {
                    case 0:
                        iScore = 8;
                        break;
                    case 1:
                        iScore = 20;
                        break;
                    case 2:
                        iScore = 35;
                        break;
                    case 3:
                        iScore = 43;
                        break;
                    case 4:
                        iScore = 57;
                        break;
                    case 5:
                        iScore = 68;
                        break;
                    case 6:
                        iScore = 73;
                        break;
                    case 7:
                        iScore = 82;
                        break;
                    case 8:
                        iScore = 87;
                        break;
                    case 9:
                        iScore = 91;
                        break;
                    case 10:
                        iScore = 96;
                        break;
                    case 11:
                        iScore = 96;
                        break;
                    case 12:
                        iScore = 96;
                        break;
                    case 13:
                        iScore = 96;
                        break;
                    case 14:
                        iScore = 96;
                        break;
                    case 15:
                        iScore = 96;
                        break;
                    case 16:
                        iScore = 96;
                        break;
                    case 17:
                        iScore = 100;
                        break;
                    case 18:
                        iScore = 100;
                        break;
                    case 19:
                        iScore = 100;
                        break;
                    case 20:
                        iScore = 100;
                        break;
                    case 21:
                        iScore = 100;
                        break;
                }
                switch (sScore)
                {
                    case 0:
                        sScore = 11;
                        break;
                    case 1:
                        sScore = 21;
                        break;
                    case 2:
                        sScore = 30;
                        break;
                    case 3:
                        sScore = 38;
                        break;
                    case 4:
                        sScore = 45;
                        break;
                    case 5:
                        sScore = 55;
                        break;
                    case 6:
                        sScore = 60;
                        break;
                    case 7:
                        sScore = 77;
                        break;
                    case 8:
                        sScore = 75;
                        break;
                    case 9:
                        sScore = 79;
                        break;
                    case 10:
                        sScore = 85;
                        break;
                    case 11:
                        sScore = 89;
                        break;
                    case 12:
                        sScore = 96;
                        break;
                    case 13:
                        sScore = 96;
                        break;
                    case 14:
                        sScore = 96;
                        break;
                    case 15:
                        sScore = 96;
                        break;
                    case 16:
                        sScore = 96;
                        break;
                    case 17:
                        sScore = 96;
                        break;
                    case 18:
                        sScore = 96;
                        break;
                    case 19:
                        sScore = 100;
                        break;
                    case 20:
                        sScore = 100;
                        break;
                    case 21:
                        sScore = 100;
                        break;
                }
                switch (cScore)
                {
                    case 0:
                        cScore = 0;
                        break;
                    case 1:
                        cScore = 16;
                        break;
                    case 2:
                        cScore = 30;
                        break;
                    case 3:
                        cScore = 40;
                        break;
                    case 4:
                        cScore = 55;
                        break;
                    case 5:
                        cScore = 66;
                        break;
                    case 6:
                        cScore = 73;
                        break;
                    case 7:
                        cScore = 85;
                        break;
                    case 8:
                        cScore = 87;
                        break;
                    case 9:
                        cScore = 97;
                        break;
                    case 10:
                        cScore = 97;
                        break;
                    case 11:
                        cScore = 97;
                        break;
                    case 12:
                        cScore = 97;
                        break;
                    case 13:
                        cScore = 97;
                        break;
                    case 14:
                        cScore = 97;
                        break;
                    case 15:
                        cScore = 100;
                        break;
                    case 16:
                        cScore = 100;
                        break;
                    case 17:
                        cScore = 100;
                        break;
                    case 18:
                        cScore = 100;
                        break;
                    case 19:
                        cScore = 100;
                        break;
                    case 20:
                        cScore = 100;
                        break;
                    case 21:
                        cScore = 100;
                        break;
                }
                //El mayor puntaje sera el que dara el perfil
                int may = int.MaxMagnitude(int.MaxMagnitude(dScore, iScore), int.MaxMagnitude(sScore, cScore));
                //Segun el mayor de los numeros se devolvera un numero que representara el resultado de la evaluacion 
                if (dScore == may) return 1;
                if (iScore == may) return 2;
                if (sScore == may) return 3;
                if (cScore == may) return 4;
                else return 0;
            }catch(Exception e) 
            { 
                return -1; 
            }
        }
    }
}