using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MAVE.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDITORY",
                columns: table => new
                {
                    AuditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    OldValue = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    NewValue = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDITORY", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_ARTICLE_TYPE",
                columns: table => new
                {
                    TypeId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleType = table.Column<string>(type: "nchar(120)", fixedLength: true, maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_ARTICLE_TYPE", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_EVALUATION",
                columns: table => new
                {
                    EvaluationId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Result = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_EVALUATION", x => x.EvaluationId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_QUESTION",
                columns: table => new
                {
                    CatQuestionId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Initial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_QUESTION", x => x.CatQuestionId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_ROLE",
                columns: table => new
                {
                    RoleId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_ROLE", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_SCORE",
                columns: table => new
                {
                    ScoreId = table.Column<short>(type: "smallint", nullable: false),
                    ScoreType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_SCORE", x => x.ScoreId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_STATUS",
                columns: table => new
                {
                    StatusId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_STATUS", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "CAT_OPTION",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaOption = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false),
                    CatQuestionId = table.Column<short>(type: "smallint", nullable: false),
                    Abcd = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CAT_OPTION", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_CAT_OPTION_CAT_QUESTION",
                        column: x => x.CatQuestionId,
                        principalTable: "CAT_QUESTION",
                        principalColumn: "CatQuestionId");
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    Phone = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    Password = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    EvaluationId = table.Column<short>(type: "smallint", nullable: false),
                    StatusId = table.Column<short>(type: "smallint", nullable: false),
                    HealthProfessionalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_USER_CAT_EVALUATION",
                        column: x => x.EvaluationId,
                        principalTable: "CAT_EVALUATION",
                        principalColumn: "EvaluationId");
                    table.ForeignKey(
                        name: "FK_USER_CAT_ROLE",
                        column: x => x.RoleId,
                        principalTable: "CAT_ROLE",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_USER_CAT_STATUS",
                        column: x => x.StatusId,
                        principalTable: "CAT_STATUS",
                        principalColumn: "StatusId");
                    table.ForeignKey(
                        name: "FK_USER_USER",
                        column: x => x.HealthProfessionalId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ARTICLE",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ArticleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Resume = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeId = table.Column<short>(type: "smallint", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ARTICLE", x => x.ArticleId);
                    table.ForeignKey(
                        name: "FK_ARTICLE_CAT_ARTICLE_TYPE",
                        column: x => x.TypeId,
                        principalTable: "CAT_ARTICLE_TYPE",
                        principalColumn: "TypeId");
                    table.ForeignKey(
                        name: "FK_ARTICLE_USER",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "MOOD",
                columns: table => new
                {
                    MoodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MoodScore = table.Column<short>(type: "smallint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOOD", x => x.MoodId);
                    table.ForeignKey(
                        name: "FK_MOOD_USER",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "QUESTION",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoreId = table.Column<short>(type: "smallint", nullable: true),
                    CatQuestionId = table.Column<short>(type: "smallint", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HABIT", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_HABIT_CAT_QUESTION",
                        column: x => x.CatQuestionId,
                        principalTable: "CAT_QUESTION",
                        principalColumn: "CatQuestionId");
                    table.ForeignKey(
                        name: "FK_QUESTION_CAT_OPTION",
                        column: x => x.OptionId,
                        principalTable: "CAT_OPTION",
                        principalColumn: "OptionId");
                    table.ForeignKey(
                        name: "FK_QUESTION_CAT_SCORE",
                        column: x => x.ScoreId,
                        principalTable: "CAT_SCORE",
                        principalColumn: "ScoreId");
                    table.ForeignKey(
                        name: "FK_QUESTION_USER",
                        column: x => x.UserId,
                        principalTable: "USER",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "CAT_ARTICLE_TYPE",
                columns: new[] { "TypeId", "ArticleType" },
                values: new object[,]
                {
                    { (short)1, "Artículo" },
                    { (short)2, "Video" },
                    { (short)3, "Podcast" },
                    { (short)4, "Ejercicio" }
                });

            migrationBuilder.InsertData(
                table: "CAT_EVALUATION",
                columns: new[] { "EvaluationId", "Result" },
                values: new object[,]
                {
                    { (short)1, "Sin evaluación" },
                    { (short)2, "Dominante" },
                    { (short)3, "Influyente" },
                    { (short)4, "Estable" },
                    { (short)5, "Concienzudo" }
                });

            migrationBuilder.InsertData(
                table: "CAT_QUESTION",
                columns: new[] { "CatQuestionId", "Initial", "Question" },
                values: new object[,]
                {
                    { (short)1, true, "Amable, gentil" },
                    { (short)2, true, "Atractivo, simpático, agradable para los demás" },
                    { (short)3, true, "Fácil de guiar, seguidor" },
                    { (short)4, true, "Abierto de mente, receptivo" },
                    { (short)5, true, "Jovial, bromista" },
                    { (short)6, true, "Competitivo, busca ganar" },
                    { (short)7, true, "Exigente, difícil de satisfacer" },
                    { (short)8, true, "Valiente, temerario, lleno de coraje" },
                    { (short)9, true, "Sociable, disfruta de la compañía de otros" },
                    { (short)10, true, "De bajo perfil, tibio, reservado" },
                    { (short)11, true, "Expresivo, habla mucho" },
                    { (short)12, true, "Refinado, elegante al hablar" },
                    { (short)13, true, "Agresivo, desafiante, enfocado a la acción" },
                    { (short)14, true, "Cuidadoso, desconfiado, cauteloso" },
                    { (short)15, true, "Entusiasta, le pone ganas" },
                    { (short)16, true, "Tiene autoconfianza, cree en sí mismo, seguro" },
                    { (short)17, true, "Muy disciplinado, auto controlado" },
                    { (short)18, true, "Admirable, digno de reconocimiento" },
                    { (short)19, true, "Respetuoso, trata con consideración a las personas" },
                    { (short)20, true, "Discutidor, polémico, confrontante" },
                    { (short)21, true, "Confía en los demás, tiene fe en la gente" },
                    { (short)22, true, "Socialmente hábil, le gusta estar con los demás" },
                    { (short)23, true, "Agradable, da gusto estar con él o ella" },
                    { (short)24, true, "Impaciente, no puede relajarse, no descansa" },
                    { (short)25, false, "¿Qué tan bien comiste hoy?" },
                    { (short)26, false, "¿Qué tanto ejercicio hiciste hoy?" },
                    { (short)27, false, "¿Descansaste bien anoche (dormiste 8 horas)?" },
                    { (short)28, false, "¿Qué tan relajado te sentiste hoy?" },
                    { (short)29, false, "¿Leíste algo el día de hoy?" },
                    { (short)30, false, "¿Aprendiste algo nuevo hoy?" },
                    { (short)31, false, "¿Qué tanto socializaste el día de hoy?" },
                    { (short)32, false, "¿Cumpliste con tus tareas de hoy?" },
                    { (short)33, false, "¿Qué tanto te has hidratado el día de hoy?" },
                    { (short)34, false, "¿Qué tan solo te sentiste el día de hoy?" },
                    { (short)35, false, "¿Te comunicaste con un ser querido hoy?" },
                    { (short)36, false, "¿Qué tanto tiempo le dedicaste a tu hobbie hoy?" },
                    { (short)37, false, "¿Qué tanto tiempo le dedicaste a tus vicios hoy?" }
                });

            migrationBuilder.InsertData(
                table: "CAT_ROLE",
                columns: new[] { "RoleId", "Role" },
                values: new object[,]
                {
                    { (short)1, "SuperAdmin" },
                    { (short)2, "Admin" },
                    { (short)3, "Psicólogo" },
                    { (short)4, "Usuario" }
                });

            migrationBuilder.InsertData(
                table: "CAT_SCORE",
                columns: new[] { "ScoreId", "ScoreType" },
                values: new object[,]
                {
                    { (short)1, "Muy mal" },
                    { (short)2, "Mal" },
                    { (short)3, "Regular" },
                    { (short)4, "Bien" },
                    { (short)5, "Muy bien" },
                    { (short)6, "Sí" },
                    { (short)7, "No" }
                });

            migrationBuilder.InsertData(
                table: "CAT_STATUS",
                columns: new[] { "StatusId", "Status" },
                values: new object[,]
                {
                    { (short)1, "Activo" },
                    { (short)2, "Inactivo" }
                });

            migrationBuilder.InsertData(
                table: "CAT_OPTION",
                columns: new[] { "OptionId", "Abcd", "CatQuestionId", "EvaOption", "Value" },
                values: new object[,]
                {
                    { 1, "A", (short)1, "Amable, gentil", "S" },
                    { 2, "B", (short)1, "Persuasivo, convincente", "I" },
                    { 3, "C", (short)1, "Humilde, reservado, modesto", "C" },
                    { 4, "D", (short)1, "Original, innovador, diferente", "D" },
                    { 5, "A", (short)2, "Atractivo, simpático, agradable para los demás", "I" },
                    { 6, "B", (short)2, "Cooperativo, está de acuerdo con frecuencia", "C" },
                    { 7, "C", (short)2, "Terco, tenaz, combativo", "D" },
                    { 8, "D", (short)2, "Dulce, complaciente", "S" },
                    { 9, "A", (short)3, "Fácil de guiar, seguidor", "C" },
                    { 10, "B", (short)3, "Directo, retador", "D" },
                    { 11, "C", (short)3, "Leal, devoto, fiel", "S" },
                    { 12, "D", (short)3, "Encantador, hace que la gente disfrute", "I" },
                    { 13, "A", (short)4, "Abierto de mente, receptivo", "C" },
                    { 14, "B", (short)4, "Considerado, gusta de ayudar", "S" },
                    { 15, "C", (short)4, "Voluntarioso, carácter fuerte y decidido", "D" },
                    { 16, "D", (short)4, "Alegre, divertido", "I" },
                    { 17, "A", (short)5, "Jovial, bromista", "I" },
                    { 18, "B", (short)5, "Preciso, exacto", "C" },
                    { 19, "C", (short)5, "Decidido, determinado, audaz", "D" },
                    { 20, "D", (short)5, "Estable, de temperamento tranquilo, calmado", "S" },
                    { 21, "A", (short)6, "Competitivo, busca ganar", "D" },
                    { 22, "B", (short)6, "Considerado, cariñoso, tiene mucho tacto", "S" },
                    { 23, "C", (short)6, "Extrovertido, le encanta la diversión", "I" },
                    { 24, "D", (short)6, "Armonioso, busca el acuerdo", "C" },
                    { 25, "A", (short)7, "Exigente, difícil de satisfacer", "C" },
                    { 26, "B", (short)7, "Obediente, hace lo que se le instruye que haga", "S" },
                    { 27, "C", (short)7, "Tenaz y decidido, determinado", "D" },
                    { 28, "D", (short)7, "Bromista, juguetón, divertido", "I" },
                    { 29, "A", (short)8, "Valiente, temerario, lleno de coraje", "D" },
                    { 30, "B", (short)8, "Inspirador, estimulante, motivador", "I" },
                    { 31, "C", (short)8, "Obediente, no confrontas, cede fácilmente", "C" },
                    { 32, "D", (short)8, "Tímido, aprehensivo, callado", "S" },
                    { 33, "A", (short)9, "Sociable, disfruta de la compañía de otros", "I" },
                    { 34, "B", (short)9, "Paciente, estable, tolerante", "S" },
                    { 35, "C", (short)9, "Autosuficiente, independiente", "D" },
                    { 36, "D", (short)9, "De bajo perfil, tibio, reservado", "C" },
                    { 37, "A", (short)10, "De bajo perfil, tibio, reservado", "D" },
                    { 38, "B", (short)10, "Receptivo, abierto a sugerencias", "C" },
                    { 39, "C", (short)10, "Cordial, cálido, amistoso", "I" },
                    { 40, "D", (short)10, "Moderado, evita los extremos", "S" },
                    { 41, "A", (short)11, "Expresivo, habla mucho", "I" },
                    { 42, "B", (short)11, "Controlado, poco expresivo", "S" },
                    { 43, "C", (short)11, "Convencional, sistemático, rutinario", "C" },
                    { 44, "D", (short)11, "Decidido, cierto, firme al tomar decisiones", "D" },
                    { 45, "A", (short)12, "Refinado, elegante al hablar", "I" },
                    { 46, "B", (short)12, "Busca retos, toma riesgos", "D" },
                    { 47, "C", (short)12, "Diplomático, tiene tacto con las personas", "C" },
                    { 48, "D", (short)12, "Satisfecho, contento, cómodo", "S" },
                    { 49, "A", (short)13, "Agresivo, desafiante, enfocado a la acción", "D" },
                    { 50, "B", (short)13, "Alma de la fiesta, entretenido, extrovertido", "I" },
                    { 51, "C", (short)13, "Ingenuo, es fácil que se aprovechen de él", "S" },
                    { 52, "D", (short)13, "Temeroso, tiende a preocuparse mucho", "C" },
                    { 53, "A", (short)14, "Cuidadoso, desconfiado, cauteloso", "C" },
                    { 54, "B", (short)14, "Determinado, decidido, se para firme", "D" },
                    { 55, "C", (short)14, "Convincente, transmite seguridad", "I" },
                    { 56, "D", (short)14, "De buen carácter, cortés, gusta de satisfacer", "S" },
                    { 57, "A", (short)15, "Entusiasta, le pone ganas", "S" },
                    { 58, "B", (short)15, "Impaciente, ansioso, desesperado a veces", "D" },
                    { 59, "C", (short)15, "Amistoso, logra acuerdos, acepta", "C" },
                    { 60, "D", (short)15, "Energético, vivaz, entusiasta", "I" },
                    { 61, "A", (short)16, "Tiene autoconfianza, cree en sí mismo, seguro", "I" },
                    { 62, "B", (short)16, "Comprensivo, compasivo, apoyador de la gente", "S" },
                    { 63, "C", (short)16, "Tolerante", "C" },
                    { 64, "D", (short)16, "Asertivo, agresivo, directo", "D" },
                    { 65, "A", (short)17, "Muy disciplinado, auto controlado", "C" },
                    { 66, "B", (short)17, "Generoso, le gusta compartir", "S" },
                    { 67, "C", (short)17, "Animado, expresivo, usa muchos gestos", "I" },
                    { 68, "D", (short)17, "Persistente, no da paso atrás, se niega a perder", "D" },
                    { 69, "A", (short)18, "Admirable, digno de reconocimiento", "I" },
                    { 70, "B", (short)18, "Amable, deseoso de ayudar y de compartir", "S" },
                    { 71, "C", (short)18, "Resignado, renuncia, no lucha", "C" },
                    { 72, "D", (short)18, "Fuerte de carácter, poderoso", "D" },
                    { 73, "A", (short)19, "Respetuoso, trata con consideración a las personas", "C" },
                    { 74, "B", (short)19, "Pionero, explorador, innovador", "D" },
                    { 75, "C", (short)19, "Optimista, ve lo positivo en las cosas", "I" },
                    { 76, "D", (short)19, "Se acomoda, complaciente, listo para ayudar", "S" },
                    { 77, "A", (short)20, "Discutidor, polémico, confrontante", "D" },
                    { 78, "B", (short)20, "Adaptable, flexible", "C" },
                    { 79, "C", (short)20, "Relajado, toma las cosas con calma, tranquilo", "S" },
                    { 80, "D", (short)20, "Ligero, despreocupado, descomplicado", "I" },
                    { 81, "A", (short)21, "Confía en los demás, tiene fe en la gente", "S" },
                    { 82, "B", (short)21, "Contento, satisfecho", "I" },
                    { 83, "C", (short)21, "Positivo, no admite dudas ni temores", "D" },
                    { 84, "D", (short)21, "Pacífico, tranquilo", "C" },
                    { 85, "A", (short)22, "Socialmente hábil, le gusta estar con los demás", "I" },
                    { 86, "B", (short)22, "Educado, culto, conocedor", "C" },
                    { 87, "C", (short)22, "Vigoroso, energético", "D" },
                    { 88, "D", (short)22, "Tolerante, poco estricto, comprensivo", "S" },
                    { 89, "A", (short)23, "Agradable, da gusto estar con él o ella", "I" },
                    { 90, "B", (short)23, "Exacto, correcto, preciso", "C" },
                    { 91, "C", (short)23, "Tiene opiniones claras, habla libre y abiertamente", "D" },
                    { 92, "D", (short)23, "Reservado, controlado, parco", "S" },
                    { 93, "A", (short)24, "Impaciente, no puede relajarse, no descansa", "D" },
                    { 94, "B", (short)24, "Sociable, amable", "S" },
                    { 95, "C", (short)24, "Popular, apreciado por mucha gente", "I" },
                    { 96, "D", (short)24, "Ordenado, organizado, nítido", "C" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARTICLE_TypeId",
                table: "ARTICLE",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ARTICLE_UserId",
                table: "ARTICLE",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CAT_OPTION_CatQuestionId",
                table: "CAT_OPTION",
                column: "CatQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MOOD_UserId",
                table: "MOOD",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_CatQuestionId",
                table: "QUESTION",
                column: "CatQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_OptionId",
                table: "QUESTION",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_ScoreId",
                table: "QUESTION",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_UserId",
                table: "QUESTION",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_EvaluationId",
                table: "USER",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_HealthProfessionalId",
                table: "USER",
                column: "HealthProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_RoleId",
                table: "USER",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_StatusId",
                table: "USER",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARTICLE");

            migrationBuilder.DropTable(
                name: "AUDITORY");

            migrationBuilder.DropTable(
                name: "MOOD");

            migrationBuilder.DropTable(
                name: "QUESTION");

            migrationBuilder.DropTable(
                name: "CAT_ARTICLE_TYPE");

            migrationBuilder.DropTable(
                name: "CAT_OPTION");

            migrationBuilder.DropTable(
                name: "CAT_SCORE");

            migrationBuilder.DropTable(
                name: "USER");

            migrationBuilder.DropTable(
                name: "CAT_QUESTION");

            migrationBuilder.DropTable(
                name: "CAT_EVALUATION");

            migrationBuilder.DropTable(
                name: "CAT_ROLE");

            migrationBuilder.DropTable(
                name: "CAT_STATUS");
        }
    }
}
