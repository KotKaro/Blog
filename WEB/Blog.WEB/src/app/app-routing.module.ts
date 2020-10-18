import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthenticatedActivator } from './activators/authenticated.activator';
import { AboutMeComponent } from './components/about-me/about-me.component';
import { LoginComponent } from './components/login/login.component';
import { ManagementPanelComponent } from './components/managment-panel/management-panel.component';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { PostDetailsComponent } from './components/post-details/post-details.component';
import { PostEditComponent } from './components/post-edit/post-edit.component';
import { PostListComponent } from './components/post-list/post-list.component';
import { PostListResolver } from './resolvers/post-list.resolver';
import { PostResolver } from './resolvers/post.resolver';

const routes: Routes = [
  {
    path: 'about-me',
    component: AboutMeComponent
  },
  {
    path: 'blog',
    component: PostListComponent,
    resolve: {
      posts: PostListResolver
    },
  },
  {
    //Create separate module for posts
    path: 'post-create',
    component: PostCreateComponent,
    canActivate: [AuthenticatedActivator]
  },
  {
    path: 'post/edit/:id',
    component: PostEditComponent,
    resolve: {
      post: PostResolver
    },
    canActivate: [AuthenticatedActivator]
  },
  {
    path: 'post/:id',
    component: PostDetailsComponent,
    resolve: {
      post: PostResolver
    }
  },
  {
    path: 'management',
    component: ManagementPanelComponent,
    canActivate: [AuthenticatedActivator]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    redirectTo: 'blog',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
