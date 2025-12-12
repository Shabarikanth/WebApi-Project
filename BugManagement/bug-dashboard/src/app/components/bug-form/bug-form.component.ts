import { Component } from '@angular/core';
import { BugService } from '../../services/bug.service';
import { Bug } from '../../models/bug';

@Component({
  selector: 'app-bug-form',
  templateUrl: './bug-form.component.html',
  styleUrls: ['./bug-form.component.css']
})
export class BugFormComponent {

  bug: Bug = {
    id: 0,
    title: '',
    description: '',
    status: 'Open',
    severity: 'Low',
    assignedTo: '',
    createdAt: '',
    updatedAt: ''
  };

  constructor(private bugService: BugService) { }

  save() {
    this.bugService.create(this.bug).subscribe({
      next: () => {
        alert('Bug created successfully!');
        this.resetForm();
      },
      error: err => console.log(err)
    });
  }

  resetForm() {
    this.bug = {
      id: 0,
      title: '',
      description: '',
      status: 'Open',
      severity: 'Low',
      assignedTo: '',
      createdAt: '',
      updatedAt: ''
    };
  }
}
