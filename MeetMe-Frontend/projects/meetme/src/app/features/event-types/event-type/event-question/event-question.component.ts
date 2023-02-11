import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IEventTypeQuestion, IUpdateEventQuestionCommand } from 'projects/meetme/src/app/models/eventtype';
import { EventTypeService } from 'projects/meetme/src/app/services/eventtype.service';

@Component({
  selector: 'app-event-questio  n',
  templateUrl: './event-question.component.html',
  styleUrls: ['./event-question.component.css']
})
export class EventQuestionComponent implements OnInit {
  eventTypeId: string = "";
  eventTypeQuestions: IModelQuestionItem[] = [];
  questionType: string = "Text";
  selectedQuestion: IModelQuestionItem | null = null;
  OPTION_JOINING_CHAR: string = "~~"
  
  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute
  ) {

    this.route.parent?.params.subscribe(params => {
      this.eventTypeId = params["id"];
      this.loadEventTypeQuestions(this.eventTypeId)

    });
  }

  ngOnInit(): void {
  }

  loadEventTypeQuestions(id: string) {
    this.eventTypeService.getEventQuetions(id).subscribe(response => {
      this.eventTypeQuestions = response.map((e => this.converToModel(e)));
      console.log(response);
    })
  }

  onAddNewQuestion() {
    this.selectedQuestion = {
      name: "",
      eventTypeId: this.eventTypeId,
      questionType: "Text",
      requiredYN: true,
      options: [{ text: "" }, { text: "" }, { text: "" }],
      otherOptionYN: false,
      activeYN: true,
      displayOrder: this.eventTypeQuestions.length

    };
  }
  onAddOption() {
    this.selectedQuestion?.options.push({ text: '' });
  }
  onRemoveOption(index: number) {
    this.selectedQuestion?.options.splice(index, 1);
  }
  onSave() {

    let listQuestonEntities = this.eventTypeQuestions.map(modelItem => {
      let entityItem: IEventTypeQuestion = {
        id: modelItem.id,
        name: modelItem.name,
        eventTypeId: modelItem.eventTypeId,
        questionType: modelItem.questionType,
        options: modelItem.options.map(e => e.text).join(this.OPTION_JOINING_CHAR),
        otherOptionYN: modelItem.otherOptionYN,
        requiredYN: modelItem.requiredYN,
        activeYN: modelItem.activeYN,
        displayOrder: modelItem.displayOrder

      };
      return entityItem;
    });

    let updateCommand: IUpdateEventQuestionCommand = {
      eventTypeId: this.eventTypeId,
      questions: listQuestonEntities
    };

    this.eventTypeService.updateEventQuestions(updateCommand).subscribe(response => {
      alert("Data saved successfully");
    })

  }

  onCancelChanges() {
    this.loadEventTypeQuestions(this.eventTypeId);
  }

  onAddQuestion(fromQuestionDetails: any) {
    if (fromQuestionDetails.invalid) return;

    this.eventTypeQuestions.splice(this.selectedQuestion?.displayOrder!, 1, this.selectedQuestion!);

    this.selectedQuestion = null;

  }

  onEditQuestion(itemToEdit: IModelQuestionItem, index: number) {
    itemToEdit.displayOrder = index;
    this.selectedQuestion =Object.assign({},itemToEdit);
  }

  onCancelEditing() {
    this.selectedQuestion = null;
  }

  converToModel(itemToEdit: IEventTypeQuestion): IModelQuestionItem {
    let optionItems: IModelOptionItem[] = [];
    if (itemToEdit.questionType !== "Text" && itemToEdit.questionType !== "MultilineText") {
      optionItems = itemToEdit.options.split(this.OPTION_JOINING_CHAR).map(e => {
        let optionItem: IModelOptionItem = { text: e };
        return optionItem;
      });
    }
    let model: IModelQuestionItem = {
      eventTypeId: itemToEdit.eventTypeId,
      name: itemToEdit.name,
      questionType: itemToEdit.questionType,
      requiredYN: itemToEdit.requiredYN,
      activeYN: itemToEdit.activeYN,
      options: optionItems,
      otherOptionYN: itemToEdit.otherOptionYN,
      displayOrder: itemToEdit.displayOrder
    }
    return model;
  }
}

export interface IModelQuestionItem {
  id?: string,
  name: string,
  eventTypeId?: string,
  questionType: string,
  requiredYN: boolean,
  options: IModelOptionItem[],
  otherOptionYN: boolean,
  activeYN: boolean,
  displayOrder: number
}
export interface IModelOptionItem {
  text: string
}

