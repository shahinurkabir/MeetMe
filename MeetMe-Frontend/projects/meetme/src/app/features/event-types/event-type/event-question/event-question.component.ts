import {  Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AlertService, CommonFunction, EventTypeService, IEventTypeQuestion, IUpdateEventQuestionCommand, ModalService,  settings_question_option_joining_char } from 'projects/meetme/src/app/app-core';

import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-event-questio  n',
  templateUrl: './event-question.component.html',
  styleUrls: ['./event-question.component.css']
})
export class EventQuestionComponent implements OnInit {
  eventTypeId: string = "";
  listSystemDefinedQuestion: IModelQuestionItem[] = [];
  listGeneralQuestion: IModelQuestionItem[] = [];
  questionType: string = "Text";
  selectedQuestion: IModelQuestionItem | undefined;
  selectedQuestionIndex: number = -1;
  @ViewChild(NgForm) fromQuestionDetails: NgForm | undefined;
  componentDestroyed$: Subject<boolean> = new Subject()
  MODAL_ID_EVENT_QUESTION: string = "eventtype-question-modal";

  constructor(
    private eventTypeService: EventTypeService,
    private route: ActivatedRoute,
    private modalService: ModalService,
    private alertService: AlertService
  ) {
    this.route.parent?.params.subscribe(params => {
      this.eventTypeId = params["id"];
      this.loadEventQuestions(this.eventTypeId)

    });
  }

  ngOnInit(): void {
  }

  loadEventQuestions(id: string) {
    this.eventTypeService.getEventQuetions(id)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe({
        next: (response: IEventTypeQuestion[]) => {
          let listEventQuestion = response.map((e => this.convertToModel(e)));
          this.listSystemDefinedQuestion = listEventQuestion.filter(e => e.systemDefinedYN == true);
          this.listGeneralQuestion = listEventQuestion.filter(e => e.systemDefinedYN == false);
          console.log(response);
        },
        error: (error) => { console.log(error) },
        complete: () => { }
      });
  }

  onAddNewQuestion() {
    this.selectedQuestion = {
      name: "",
      eventTypeId: this.eventTypeId,
      questionType: "Text",
      requiredYN: false,
      options: [{ text: "" }, { text: "" }, { text: "" }],
      otherOptionYN: false,
      activeYN: false,
      displayOrder: this.listGeneralQuestion.length,
      systemDefinedYN: false
    };
    this.modalService.open(this.MODAL_ID_EVENT_QUESTION);
  }
  onRemoveQuestion(e:any) {
    e.preventDefault();
    if (this.selectedQuestionIndex < 0) return;
    this.listGeneralQuestion.splice(this.selectedQuestionIndex, 1);
    this.selectedQuestion = undefined;
    this.selectedQuestionIndex = -1;
    this.modalService.close();

  }
  onAddOption(e:any) {
    e.preventDefault();
    this.selectedQuestion?.options.push({ text: '' });
  }
  onRemoveOption(index: number) {
    this.selectedQuestion?.options.splice(index, 1);
  }

  onSave() {

    let listQuestonEntities = this.listGeneralQuestion.map(modelItem => {
      let entityItem: IEventTypeQuestion = {
        id: modelItem.id,
        name: modelItem.name,
        eventTypeId: modelItem.eventTypeId,
        questionType: modelItem.questionType,
        options: modelItem.options.map(e => e.text).join(settings_question_option_joining_char),
        otherOptionYN: modelItem.otherOptionYN,
        requiredYN: modelItem.requiredYN,
        activeYN: modelItem.activeYN,
        displayOrder: modelItem.displayOrder,
        systemDefinedYN: modelItem.systemDefinedYN

      };
      return entityItem;
    });

    let updateCommand: IUpdateEventQuestionCommand = {
      eventTypeId: this.eventTypeId,
      questions: listQuestonEntities
    };

    this.eventTypeService.updateEventQuestions(updateCommand)
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        {
          next: response => {
            this.alertService.success("Event questions updated successfully");
          },
          error: (error) => { console.log(error) },
          complete: () => { }
        })

  }

  onCancelChanges() {
    this.loadEventQuestions(this.eventTypeId);
  }
  onEditQuestion(itemToEdit: IModelQuestionItem, index: number) {
    itemToEdit.displayOrder = index;
    this.selectedQuestionIndex = index;
    this.selectedQuestion =CommonFunction.cloneObject(itemToEdit);
    this.modalService.open(this.MODAL_ID_EVENT_QUESTION);
  }

  onCancelEditing() {
    this.selectedQuestion = undefined;
    this.selectedQuestionIndex = -1;
    this.modalService.close();
  }

  onConfirmEditQuestion(e: any) {
    if (!this.fromQuestionDetails || this.fromQuestionDetails.invalid) return;

    if (this.selectedQuestion?.questionType !== "RadioButtons" && this.selectedQuestion?.questionType !== "CheckBoxes") {
      this.selectedQuestion!.otherOptionYN = false;
    }

    this.listGeneralQuestion.splice(this.selectedQuestion?.displayOrder!, 1, this.selectedQuestion!);

    this.selectedQuestion = undefined;

    this.modalService.close();

  }

  // question drag and drop
  dragQuestionItemIndex: number = 0;
  onDragStartQuestion(index: number): void {
    this.dragQuestionItemIndex = index;
  }

  onDragOverQuestion(event: DragEvent): void {
    event.preventDefault();
  }

  onDropQuestion(index: number): void {
    const targetIndex = index;
    this.swapQuestionItems(this.dragQuestionItemIndex, targetIndex);
  }

  swapQuestionItems(sourceIndex: number, targetIndex: number): void {

    let sourceItem: IModelQuestionItem = Object.assign({}, this.listGeneralQuestion[sourceIndex]);
    let targetItem: IModelQuestionItem = Object.assign({}, this.listGeneralQuestion[targetIndex]);

    this.listGeneralQuestion[sourceIndex] = targetItem;
    this.listGeneralQuestion[targetIndex] = sourceItem;

  }

  // question drag and drop
  dragItemIndex: number = 0;

  onDragStart(event: DragEvent): void {
    this.dragItemIndex = +(event.target as HTMLElement).id;
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  onDrop(event: DragEvent): void {
    const targetIndex = +(event.target as HTMLElement).id;
    this.swapItems(this.dragItemIndex, targetIndex);
  }
  
  swapItems(sourceIndex: number, targetIndex: number): void {

    if (this.selectedQuestion?.options === undefined) return;

    if (this.selectedQuestion!.options.length <= 1) return;

    let options = [... this.selectedQuestion?.options];
    let sourceItem: IModelOptionItem = Object.assign({}, options[sourceIndex]);
    let targetItem: IModelOptionItem = Object.assign({}, options[targetIndex]);

    options[sourceIndex] = targetItem;
    options[targetIndex] = sourceItem;

    this.selectedQuestion.options = options;
  }

  convertToModel(entityItem: IEventTypeQuestion): IModelQuestionItem {
    let optionItems: IModelOptionItem[] = [];
    if (entityItem.questionType !== "Text" && entityItem.questionType !== "MultilineText") {
      optionItems = entityItem.options.split(settings_question_option_joining_char).map(e => {
        let optionItem: IModelOptionItem = { text: e };
        return optionItem;
      });
    }
    let model: IModelQuestionItem = {
      id: entityItem.id,
      eventTypeId: entityItem.eventTypeId,
      name: entityItem.name,
      questionType: entityItem.questionType,
      requiredYN: entityItem.requiredYN,
      activeYN: entityItem.activeYN,
      options: optionItems,
      otherOptionYN: entityItem.otherOptionYN,
      displayOrder: entityItem.displayOrder,
      systemDefinedYN: entityItem.systemDefinedYN

    }
    return model;
  }
  onChangedOptionText(optionItem: IModelOptionItem) {
    if (optionItem.text.trim() === "") {
      optionItem.validationMessage = "Option text is required";
    }
    else {
      optionItem.validationMessage = "";
    }
  }
  ngOnDestroy() {
    this.componentDestroyed$.next(true)
    this.componentDestroyed$.complete()
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
  displayOrder: number,
  systemDefinedYN: boolean
}
export interface IModelOptionItem {
  text: string,
  validationMessage?: string

}

