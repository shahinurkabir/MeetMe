// import { Component, OnInit } from '@angular/core';
// import { ScheduleRule } from '../../interfaces/schedule-rule';
// import { WorkinghourService } from '../../services/workinghour.service';

// @Component({
//   selector: 'app-workinghours',
//   templateUrl: './workinghours.component.html',
//   styleUrls: ['./workinghours.component.css']
// })
// export class WorkinghoursComponent implements OnInit {
//   listWorkingSchedules:ScheduleRule[]=[];
//   constructor(private workingScheduleServices:WorkinghourService) { }

//   ngOnInit(): void {
//     this.loadWorkingSchedules();
//   }

//   loadWorkingSchedules() {
//     this.workingScheduleServices.getList().subscribe(response=>{
//       this.listWorkingSchedules=response;
//     })
//   }
// } 
