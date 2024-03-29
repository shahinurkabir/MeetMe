import { NgFor } from '@angular/common';
import { Component, ElementRef, OnDestroy, OnInit, Renderer2, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AvailabilityListComponent } from './availability-list/availability-list.component';
import { Subject, takeUntil } from 'rxjs';
import { TimeAvailabilityComponent, IAvailability, AvailabilityService, AlertService, IEditAvailabilityNameCommand, ICloneAvailabilityCommand, IEditAvailabilityCommand, IDeleteAvailabilityCommand, ISetDefaultAvailabilityCommand } from '../../app-core';

@Component({
  selector: 'app-availability',
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss']
})
export class AvailabilityComponent implements OnInit, OnDestroy {
  destroyed$: Subject<boolean> = new Subject<boolean>();
  @ViewChild("availabilityComponent", { static: true }) timeAvailabilityComponent: TimeAvailabilityComponent | undefined;
  @ViewChild("listAvailabilityComponent", { static: true }) listAvailabilityComponent: AvailabilityListComponent | undefined;
  @ViewChild(NgForm) editNameForm: NgForm | undefined;
  @ViewChild('toggle_availability_action_menu') toggleButton: ElementRef | undefined;
  @ViewChild('availability_action_menu') menu: ElementRef | undefined

  editName: string = "";
  selectedAvailability: IAvailability | undefined;
  showActionMenubarYN: boolean = false;
  isLoading: boolean=false;
  isSaving: boolean=false;
  showDeleteModalYN: boolean=false;
  showEditModalYN: boolean=false;
  submitted: boolean=false;
  constructor(
    private availabilityService: AvailabilityService,
    private renderer: Renderer2,
    private alertService: AlertService
  ) {
    this.renderer.listen('window', 'click', (e: Event) => {

      console.log(e.target);
      if (e.target != this.toggleButton?.nativeElement && e.target != this.menu?.nativeElement) {
        this.showActionMenubarYN = false;
      }

    });
  }

  ngOnInit(): void {
  }
  onSelected(availability: any) {
    this.selectedAvailability = availability;
    this.selectedAvailability = Object.assign({}, availability);
    this.timeAvailabilityComponent?.setAvailability(this.selectedAvailability!);

  }

  onOpenActionMenu() {
    this.showActionMenubarYN = !this.showActionMenubarYN;
  }

  onClickEditName(id: string) {
    this.resetForm(this.editNameForm);
    this.editName = this.selectedAvailability?.name!
    this.showEditModalYN = true;  
    this.submitted = false;
  }

  // onEditName(e: any) {
  //   this.editNameForm?.onSubmit(e)
  // }

  onEditNameFormSubmit(frm: NgForm) {
    this.submitted = true;
    if (frm.invalid) return;

    let command: IEditAvailabilityNameCommand = {
      id: this.selectedAvailability?.id!,
      name: this.editName,
    };
    this.availabilityService.editName(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Availability name updated successfully");
          this.listAvailabilityComponent?.loadData(this.selectedAvailability?.id)
        },
        error: (error) => { console.log(error) },
        complete: () => { this.onCloseModal(); }
      });
  }

  onCloseModal() {
    this.showDeleteModalYN = false;
    this.showEditModalYN = false;
  }

  onClone(id: string) {
    let command: ICloneAvailabilityCommand = {
      id: id
    };

    this.availabilityService.clone(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Availability cloned successfully");
          this.listAvailabilityComponent?.loadData(response);
        },
        error: (error) => {
          // todo display error
          console.log(error);
        },
        complete: () => { }
      })
  }

  onSubmit(event?: any) {
    event?.preventDefault();
    let availability = this.timeAvailabilityComponent?.getAvailability();

    let command: IEditAvailabilityCommand = {
      id: availability?.id!,
      name: availability?.name!,
      timeZone: availability?.timeZone!,
      details: availability?.details!
    }
    this.isSaving = true;
    this.availabilityService.edit(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Availability updated successfully");
          this.listAvailabilityComponent?.loadData(this.selectedAvailability?.id);
        },
        error: (error) => { console.log(error) },
        complete: () => { this.isSaving = false;}
      });
  }

  onShowDeleteModal() {
    this.showDeleteModalYN =true;
  }

  onDelete(e: any) {
    let command: IDeleteAvailabilityCommand = {
      id: this.selectedAvailability?.id!
    };

    this.availabilityService.delete(command)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: response => {
          this.alertService.success("Availability deleted successfully");
          this.listAvailabilityComponent?.loadData(undefined);
        },
        error: (error) => { console.log(error) },
        complete: () => { this.onCloseModal(); }
      })
  }

  onDefault(id: string) {
    let command: ISetDefaultAvailabilityCommand = {
      id: id
    };

    this.availabilityService.setDefault(command).subscribe({
      next: response => {
        this.alertService.success("Availability set as default successfully");
        this.listAvailabilityComponent?.loadData(this.selectedAvailability?.id)
      },
      error: (error) => { console.log(error) }
    })
  }
  onTimeAvailabilityChanged(isValidChanged: boolean) {
    if (isValidChanged) {
      this.onSubmit();
    }
  }

  private resetForm(frm: NgForm | undefined) {
    frm?.form.markAsPristine();
    frm?.resetForm();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}
