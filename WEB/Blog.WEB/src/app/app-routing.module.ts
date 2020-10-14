import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AboutMeComponent } from './components/about-me/about-me.component';
import { BlogComponent } from './components/blog/blog.component';
import { LoginComponent } from './components/login/login.component';
import { ManagementPanelComponent } from './components/managment-panel/management-panel.component';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { PostDetailsComponent } from './components/post-details/post-details.component';
import { PostEditComponent } from './components/post-edit/post-edit.component';
import { PostResolver } from './resolvers/post.resolver';

const routes: Routes = [
  {
    path: 'about-me',
    component: AboutMeComponent
  },
  {
    path: 'blog',
    component: BlogComponent
  },
  {
    //Create separate module for posts
    path: 'post-create',
    component: PostCreateComponent
  },
  {
    path: 'post/edit/:id',
    component: PostEditComponent,
    resolve: {
      post: PostResolver
    }
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
    component: ManagementPanelComponent
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
