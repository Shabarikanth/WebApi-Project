import { Component, OnInit } from '@angular/core';
import { BugService } from '../../services/bug.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Bug } from '../../models/bug';


@Component({
    selector: 'app-bug-form',
    templateUrl: './bug-form.component.html'
})
export class BugFormComponent implements OnInit {
    model: Partial<Bug> = { status: 'Open' };
    id?: number;
    statuses = ['Open', 'Work In Progress', 'Closed', 'Hold', 'Rejected'];

    constructor(
        private svc: BugService,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        const idStr = this.route.snapshot.paramMap.get('id');
        this.id = idStr ? Number(idStr) : undefined;
        if (this.id) {
            this.svc.get(this.id).subscribe({
                next: (b) => (this.model = b),
                error: (err) => console.error('Failed to load bug', err)
            });
        }
    }

    save() {
        if (!this.model.title || this.model.title.trim().length === 0) {
            return alert('Title is required');
        }
        if (this.id) {
            this.svc.update(this.id, this.model).subscribe({
                next: () => this.router.navigate(['/']),
                error: (err) => console.error('Update failed', err)
            });
        } else {
            this.svc.create(this.model).subscribe({
                next: () => this.router.navigate(['/']),
                error: (err) => console.error('Create failed', err)
            });
        }
    }
}
