import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { CloneAvailabilityCommand } from '../../commands/cloneAvailabilityCommand';
import { CreateAvailabilityCommand } from '../../commands/createAvailabilityCommand';
import { DeleteAvailabilityCommand, SetDefaultAvailabilityCommand } from '../../commands/deleteAvailabilityCommand';
import { EditAvailabilityCommand } from '../../commands/editAvailabilityCommand';
import { EditAvailabilityNameCommand } from '../../commands/editAvailabilityNameCommand';
import { ModalService } from '../../controls/modal/modalService';
import { TimeAvailabilityComponent } from '../../controls/time-availability/time-availability.component';
import { IAvailability } from '../../models/IAvailability';
import { AvailabilityService } from '../../services/availability.service';
import { AvailabilityListComponent } from './availability-list/availability-list.component';

@Component({
  selector: 'app-availability',
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss']
})
export class AvailabilityComponent implements OnInit {
  @ViewChild("availabilityComponent", { static: true }) timeAvailabilityComponent: TimeAvailabilityComponent | undefined;
  @ViewChild("listAvailabilityComponent", { static: true }) listAvailabilityComponent: AvailabilityListComponent | undefined;

  editName: string = "";
  selectedAvailability: IAvailability | undefined;
  showToolBox:boolean=false;
  constructor(
    private availabilityService: AvailabilityService,
    private modalService: ModalService

  ) { }

  ngOnInit(): void {
  }
  onSelected(availability: any) {
    this.selectedAvailability = availability;
    this.selectedAvailability = Object.assign({}, availability);
    this.timeAvailabilityComponent?.setAvailability(this.selectedAvailability!);
    this.timeAvailabilityComponent?.prepareWeeklyViewData();
    this.timeAvailabilityComponent?.prepareMonthlyViewData();

  }

  onClickGear() {
    this.showToolBox=!this.showToolBox;
  }
  onCloseGrearMenu(e:any) {
    this.showToolBox=!this.showToolBox;
  }
  onClickEditName(id: string) {
    this.editName = this.selectedAvailability?.name!
    this.modalService.open('edit-name-modal');
  }
  onClickDelete(id: string) {

  }
  onEditName() {
    let command: EditAvailabilityNameCommand = {
      id: this.selectedAvailability?.id!,
      name: this.editName,
    };
    this.availabilityService.editName(command).subscribe({
      next: response => {
        this.listAvailabilityComponent?.loadList()
      },
      error: (error) => { console.log(error) },
      complete: () => { this.modalService.close() }
    });
  }

  onCancelEdit() {
    this.modalService.close();
  }
  onClone(id: string) {
    let command: CloneAvailabilityCommand = {
      id: id
    };

    this.availabilityService.clone(command).subscribe({
      next: response => { this.listAvailabilityComponent?.loadList(); },
      error: (error) => {
        // todo display error
        console.log(error);
      }
    })
  }
  onSave(event: any) {
    event.preventDefault();
    let availability = this.timeAvailabilityComponent?.getAvailability();

    let command: EditAvailabilityCommand = {
      id: availability?.id!,
      name: availability?.name!,
      timeZoneId: availability?.timeZoneId!,
      details: availability?.details!
    }

    this.availabilityService.edit(command).subscribe(response => {
      console.log(response);
    });
  }

  onDelete(id: string) {
    let command: DeleteAvailabilityCommand = {
      id: id
    };

    this.availabilityService.delete(command).subscribe({
      next: response => { this.listAvailabilityComponent?.loadList(); },
      error: (error) => { console.log(error) }
    })
  }
  onDefault(id: string) {
    let command: SetDefaultAvailabilityCommand = {
      id: id
    };

    this.availabilityService.setDefault(command).subscribe({
      next: response => { this.listAvailabilityComponent?.loadList() },
      error: (error) => { console.log(error) }
    })
  }
}
