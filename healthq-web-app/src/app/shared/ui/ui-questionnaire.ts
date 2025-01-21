import { Questionnaire } from 'fhir/r5';
import { QuestionType } from '../enums/question-types';

export class UiQuestionnaire {
  questionnaire: Questionnaire;
  type: QuestionType;
}
