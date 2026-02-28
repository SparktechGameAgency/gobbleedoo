using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizDatabase", menuName = "Quiz/Quiz Database")]
public class QuizDatabase : ScriptableObject
{
    public List<Question> questions;

    [System.Serializable]
    public struct Question
    {
        [TextArea]
        public string questionText;

        public string[] options;   // Size 4

        public int correctAnswerIndex;
    }
}