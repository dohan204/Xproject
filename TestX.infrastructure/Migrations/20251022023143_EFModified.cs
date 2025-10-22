using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestX.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Histories_HistoryId1",
                table: "Choices");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamDetails_Exams_ExamId1",
                table: "ExamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Subjects_SubjectId1",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Levels_LevelId1",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId1",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExamDetails_Histories_HistoryId1",
                table: "StudentExamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExamDetails_StudentExams_StudentExamId1",
                table: "StudentExamDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExams_Exams_ExamId1",
                table: "StudentExams");

            migrationBuilder.DropIndex(
                name: "IX_StudentExams_ExamId1",
                table: "StudentExams");

            migrationBuilder.DropIndex(
                name: "IX_StudentExamDetails_HistoryId1",
                table: "StudentExamDetails");

            migrationBuilder.DropIndex(
                name: "IX_StudentExamDetails_StudentExamId1",
                table: "StudentExamDetails");

            migrationBuilder.DropIndex(
                name: "IX_Questions_LevelId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuestionTypeId1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SubjectId1",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_ExamDetails_ExamId1",
                table: "ExamDetails");

            migrationBuilder.DropIndex(
                name: "IX_Choices_HistoryId1",
                table: "Choices");

            migrationBuilder.DropColumn(
                name: "ExamId1",
                table: "StudentExams");

            migrationBuilder.DropColumn(
                name: "HistoryId1",
                table: "StudentExamDetails");

            migrationBuilder.DropColumn(
                name: "StudentExamId1",
                table: "StudentExamDetails");

            migrationBuilder.DropColumn(
                name: "LevelId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SubjectId1",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExamId1",
                table: "ExamDetails");

            migrationBuilder.DropColumn(
                name: "HistoryId1",
                table: "Choices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamId1",
                table: "StudentExams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId1",
                table: "StudentExamDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentExamId1",
                table: "StudentExamDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelId1",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionTypeId1",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId1",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamId1",
                table: "ExamDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HistoryId1",
                table: "Choices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentExams_ExamId1",
                table: "StudentExams",
                column: "ExamId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamDetails_HistoryId1",
                table: "StudentExamDetails",
                column: "HistoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamDetails_StudentExamId1",
                table: "StudentExamDetails",
                column: "StudentExamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_LevelId1",
                table: "Questions",
                column: "LevelId1");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionTypeId1",
                table: "Questions",
                column: "QuestionTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SubjectId1",
                table: "Exams",
                column: "SubjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExamDetails_ExamId1",
                table: "ExamDetails",
                column: "ExamId1");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_HistoryId1",
                table: "Choices",
                column: "HistoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Histories_HistoryId1",
                table: "Choices",
                column: "HistoryId1",
                principalTable: "Histories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamDetails_Exams_ExamId1",
                table: "ExamDetails",
                column: "ExamId1",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Subjects_SubjectId1",
                table: "Exams",
                column: "SubjectId1",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Levels_LevelId1",
                table: "Questions",
                column: "LevelId1",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId1",
                table: "Questions",
                column: "QuestionTypeId1",
                principalTable: "QuestionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExamDetails_Histories_HistoryId1",
                table: "StudentExamDetails",
                column: "HistoryId1",
                principalTable: "Histories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExamDetails_StudentExams_StudentExamId1",
                table: "StudentExamDetails",
                column: "StudentExamId1",
                principalTable: "StudentExams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExams_Exams_ExamId1",
                table: "StudentExams",
                column: "ExamId1",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
