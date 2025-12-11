import { Component, OnInit } from '@angular/core';
import { BugService } from '../../services/BugService';
import { Bug } from '../models/bug';
import { Router } from '@angular/router';


@Component({
    selector: 'app-bug-list',
    templateUrl: './bug-list.component.html'
})
export class BugListComponent implements OnInit {
    bugs: Bug[] = [];
    loading = false;
    statuses = ['Open', 'Work In Progress', 'Closed', 'Hold', 'Rejected'];

    constructor(private svc: BugService, private router: Router) { }

    ngOnInit(): void {
        this.load();
    }

    load() {
        this.loading = true;
        this.svc.getAll().subscribe({
            next: (b) => {
                this.bugs = b;
                this.loading = false;
            },
            error: (err) => {
                console.error('Failed to load bugs', err);
                this.loading = false;
            }
        });
    }

    delete(id?: number) {
        if (!id) return;
        if (!confirm('Delete this bug?')) return;
        this.svc.delete(id).subscribe({
            next: () => this.load(),
            error: (err) => console.error('Delete failed', err)
        });
    }

    edit(id?: number) {
        if (!id) return;
        this.router.navigate(['/edit', id]);
    }
}
