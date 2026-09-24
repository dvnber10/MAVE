using MAVE.Models;

namespace MAVE.Seed;

public static class SeedData
{
    // Preguntas iniciales (24). El perfil de cada opción se toma del Scoring
    // (EvaluationUtility) y el faltante por pregunta se reconstruyó para
    // completar {D, I, S, C}.
    private static readonly (string Question, (string Letter, string Text, char Value)[] Options)[] InitialRaw;
    private static readonly string[] HabitRaw;

    static SeedData()
    {
        InitialRaw = BuildInitialRaw();
        HabitRaw = BuildHabitRaw();
        Questions = BuildQuestions();
        Options = BuildOptions();
    }

    public static readonly CatRole[] Roles =
    {
        new() { RoleId = 1, Role = "SuperAdmin" },
        new() { RoleId = 2, Role = "Admin" },
        new() { RoleId = 3, Role = "Psicólogo" },
        new() { RoleId = 4, Role = "Usuario" }
    };

    public static readonly CatStatus[] Statuses =
    {
        new() { StatusId = 1, Status = "Activo" },
        new() { StatusId = 2, Status = "Inactivo" }
    };

    public static readonly CatEvaluation[] Evaluations =
    {
        new() { EvaluationId = 1, Result = "Sin evaluación" },
        new() { EvaluationId = 2, Result = "Dominante" },
        new() { EvaluationId = 3, Result = "Influyente" },
        new() { EvaluationId = 4, Result = "Estable" },
        new() { EvaluationId = 5, Result = "Concienzudo" }
    };

    public static readonly CatScore[] Scores =
    {
        new() { ScoreId = 1, ScoreType = "Muy mal" },
        new() { ScoreId = 2, ScoreType = "Mal" },
        new() { ScoreId = 3, ScoreType = "Regular" },
        new() { ScoreId = 4, ScoreType = "Bien" },
        new() { ScoreId = 5, ScoreType = "Muy bien" },
        new() { ScoreId = 6, ScoreType = "Sí" },
        new() { ScoreId = 7, ScoreType = "No" }
    };

    public static readonly CatArticleType[] ArticleTypes =
    {
        new() { TypeId = 1, ArticleType = "Artículo" },
        new() { TypeId = 2, ArticleType = "Video" },
        new() { TypeId = 3, ArticleType = "Podcast" },
        new() { TypeId = 4, ArticleType = "Ejercicio" }
    };

    public static readonly CatQuestion[] Questions;

    public static readonly CatOption[] Options;

    private static CatQuestion[] BuildQuestions()
    {
        var list = new List<CatQuestion>();
        short id = 1;
        foreach (var entry in InitialRaw)
        {
            list.Add(new CatQuestion { CatQuestionId = id, Question = entry.Question, Initial = true });
            id++;
        }
        foreach (var habit in HabitRaw)
        {
            list.Add(new CatQuestion { CatQuestionId = id, Question = habit, Initial = false });
            id++;
        }
        return list.ToArray();
    }

    private static CatOption[] BuildOptions()
    {
        var list = new List<CatOption>();
        int optionId = 1;
        short questionId = 1;
        foreach (var entry in InitialRaw)
        {
            foreach (var opt in entry.Options)
            {
                list.Add(new CatOption
                {
                    OptionId = optionId,
                    CatQuestionId = questionId,
                    Abcd = opt.Letter,
                    EvaOption = opt.Text,
                    Value = opt.Value.ToString()
                });
                optionId++;
            }
            questionId++;
        }
        return list.ToArray();
    }

    // La instrucción "return" preserva la lectura clara de los datos.
    private static (string Question, (string Letter, string Text, char Value)[] Options)[] BuildInitialRaw()
    {
        return new[]
        {
            ("Amable, gentil", new[] { ("A", "Amable, gentil", 'S'), ("B", "Persuasivo, convincente", 'I'), ("C", "Humilde, reservado, modesto", 'C'), ("D", "Original, innovador, diferente", 'D') }),
            ("Atractivo, simpático, agradable para los demás", new[] { ("A", "Atractivo, simpático, agradable para los demás", 'I'), ("B", "Cooperativo, está de acuerdo con frecuencia", 'C'), ("C", "Terco, tenaz, combativo", 'D'), ("D", "Dulce, complaciente", 'S') }),
            ("Fácil de guiar, seguidor", new[] { ("A", "Fácil de guiar, seguidor", 'C'), ("B", "Directo, retador", 'D'), ("C", "Leal, devoto, fiel", 'S'), ("D", "Encantador, hace que la gente disfrute", 'I') }),
            ("Abierto de mente, receptivo", new[] { ("A", "Abierto de mente, receptivo", 'C'), ("B", "Considerado, gusta de ayudar", 'S'), ("C", "Voluntarioso, carácter fuerte y decidido", 'D'), ("D", "Alegre, divertido", 'I') }),
            ("Jovial, bromista", new[] { ("A", "Jovial, bromista", 'I'), ("B", "Preciso, exacto", 'C'), ("C", "Decidido, determinado, audaz", 'D'), ("D", "Estable, de temperamento tranquilo, calmado", 'S') }),
            ("Competitivo, busca ganar", new[] { ("A", "Competitivo, busca ganar", 'D'), ("B", "Considerado, cariñoso, tiene mucho tacto", 'S'), ("C", "Extrovertido, le encanta la diversión", 'I'), ("D", "Armonioso, busca el acuerdo", 'C') }),
            ("Exigente, difícil de satisfacer", new[] { ("A", "Exigente, difícil de satisfacer", 'C'), ("B", "Obediente, hace lo que se le instruye que haga", 'S'), ("C", "Tenaz y decidido, determinado", 'D'), ("D", "Bromista, juguetón, divertido", 'I') }),
            ("Valiente, temerario, lleno de coraje", new[] { ("A", "Valiente, temerario, lleno de coraje", 'D'), ("B", "Inspirador, estimulante, motivador", 'I'), ("C", "Obediente, no confrontas, cede fácilmente", 'C'), ("D", "Tímido, aprehensivo, callado", 'S') }),
            ("Sociable, disfruta de la compañía de otros", new[] { ("A", "Sociable, disfruta de la compañía de otros", 'I'), ("B", "Paciente, estable, tolerante", 'S'), ("C", "Autosuficiente, independiente", 'D'), ("D", "De bajo perfil, tibio, reservado", 'C') }),
            ("De bajo perfil, tibio, reservado", new[] { ("A", "De bajo perfil, tibio, reservado", 'D'), ("B", "Receptivo, abierto a sugerencias", 'C'), ("C", "Cordial, cálido, amistoso", 'I'), ("D", "Moderado, evita los extremos", 'S') }),
            ("Expresivo, habla mucho", new[] { ("A", "Expresivo, habla mucho", 'I'), ("B", "Controlado, poco expresivo", 'S'), ("C", "Convencional, sistemático, rutinario", 'C'), ("D", "Decidido, cierto, firme al tomar decisiones", 'D') }),
            ("Refinado, elegante al hablar", new[] { ("A", "Refinado, elegante al hablar", 'I'), ("B", "Busca retos, toma riesgos", 'D'), ("C", "Diplomático, tiene tacto con las personas", 'C'), ("D", "Satisfecho, contento, cómodo", 'S') }),
            ("Agresivo, desafiante, enfocado a la acción", new[] { ("A", "Agresivo, desafiante, enfocado a la acción", 'D'), ("B", "Alma de la fiesta, entretenido, extrovertido", 'I'), ("C", "Ingenuo, es fácil que se aprovechen de él", 'S'), ("D", "Temeroso, tiende a preocuparse mucho", 'C') }),
            ("Cuidadoso, desconfiado, cauteloso", new[] { ("A", "Cuidadoso, desconfiado, cauteloso", 'C'), ("B", "Determinado, decidido, se para firme", 'D'), ("C", "Convincente, transmite seguridad", 'I'), ("D", "De buen carácter, cortés, gusta de satisfacer", 'S') }),
            ("Entusiasta, le pone ganas", new[] { ("A", "Entusiasta, le pone ganas", 'S'), ("B", "Impaciente, ansioso, desesperado a veces", 'D'), ("C", "Amistoso, logra acuerdos, acepta", 'C'), ("D", "Energético, vivaz, entusiasta", 'I') }),
            ("Tiene autoconfianza, cree en sí mismo, seguro", new[] { ("A", "Tiene autoconfianza, cree en sí mismo, seguro", 'I'), ("B", "Comprensivo, compasivo, apoyador de la gente", 'S'), ("C", "Tolerante", 'C'), ("D", "Asertivo, agresivo, directo", 'D') }),
            ("Muy disciplinado, auto controlado", new[] { ("A", "Muy disciplinado, auto controlado", 'C'), ("B", "Generoso, le gusta compartir", 'S'), ("C", "Animado, expresivo, usa muchos gestos", 'I'), ("D", "Persistente, no da paso atrás, se niega a perder", 'D') }),
            ("Admirable, digno de reconocimiento", new[] { ("A", "Admirable, digno de reconocimiento", 'I'), ("B", "Amable, deseoso de ayudar y de compartir", 'S'), ("C", "Resignado, renuncia, no lucha", 'C'), ("D", "Fuerte de carácter, poderoso", 'D') }),
            ("Respetuoso, trata con consideración a las personas", new[] { ("A", "Respetuoso, trata con consideración a las personas", 'C'), ("B", "Pionero, explorador, innovador", 'D'), ("C", "Optimista, ve lo positivo en las cosas", 'I'), ("D", "Se acomoda, complaciente, listo para ayudar", 'S') }),
            ("Discutidor, polémico, confrontante", new[] { ("A", "Discutidor, polémico, confrontante", 'D'), ("B", "Adaptable, flexible", 'C'), ("C", "Relajado, toma las cosas con calma, tranquilo", 'S'), ("D", "Ligero, despreocupado, descomplicado", 'I') }),
            ("Confía en los demás, tiene fe en la gente", new[] { ("A", "Confía en los demás, tiene fe en la gente", 'S'), ("B", "Contento, satisfecho", 'I'), ("C", "Positivo, no admite dudas ni temores", 'D'), ("D", "Pacífico, tranquilo", 'C') }),
            ("Socialmente hábil, le gusta estar con los demás", new[] { ("A", "Socialmente hábil, le gusta estar con los demás", 'I'), ("B", "Educado, culto, conocedor", 'C'), ("C", "Vigoroso, energético", 'D'), ("D", "Tolerante, poco estricto, comprensivo", 'S') }),
            ("Agradable, da gusto estar con él o ella", new[] { ("A", "Agradable, da gusto estar con él o ella", 'I'), ("B", "Exacto, correcto, preciso", 'C'), ("C", "Tiene opiniones claras, habla libre y abiertamente", 'D'), ("D", "Reservado, controlado, parco", 'S') }),
            ("Impaciente, no puede relajarse, no descansa", new[] { ("A", "Impaciente, no puede relajarse, no descansa", 'D'), ("B", "Sociable, amable", 'S'), ("C", "Popular, apreciado por mucha gente", 'I'), ("D", "Ordenado, organizado, nítido", 'C') }),
        };
    }

    // Mismo orden que la lista de preguntas del front (HabitQuestions.jsx).
    private static string[] BuildHabitRaw()
    {
        return new[]
        {
            "¿Qué tan bien comiste hoy?",
            "¿Qué tanto ejercicio hiciste hoy?",
            "¿Descansaste bien anoche (dormiste 8 horas)?",
            "¿Qué tan relajado te sentiste hoy?",
            "¿Leíste algo el día de hoy?",
            "¿Aprendiste algo nuevo hoy?",
            "¿Qué tanto socializaste el día de hoy?",
            "¿Cumpliste con tus tareas de hoy?",
            "¿Qué tanto te has hidratado el día de hoy?",
            "¿Qué tan solo te sentiste el día de hoy?",
            "¿Te comunicaste con un ser querido hoy?",
            "¿Qué tanto tiempo le dedicaste a tu hobbie hoy?",
            "¿Qué tanto tiempo le dedicaste a tus vicios hoy?",
        };
    }
}