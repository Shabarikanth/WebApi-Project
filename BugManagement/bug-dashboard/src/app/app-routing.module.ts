import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BugListComponent } from './components/bug-list/bug-list.component';
import { BugFormComponent } from './components/bug-form/bug-form.component';

const routes: Routes = [
  { path: '', component: BugListComponent },
  { path: 'create', component: BugFormComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
