import { Component, OnInit } from '@angular/core';
import { BugService } from '../../services/bug.service';
import { Bug } from '../../models/bug';

@Component({
  selector: 'app-bug-list',
  templateUrl: './bug-list.component.html',
  styleUrls: ['./bug-list.component.css']
})
export class BugListComponent implements OnInit {

  bugs: Bug[] = [];

  constructor(private bugService: BugService) { }

  ngOnInit(): void {
    this.loadBugs();
  }

  loadBugs() {
    this.bugService.getAll().subscribe({
      next: (res) => this.bugs = res,
      error: err => console.log(err)
    });
  }
}
