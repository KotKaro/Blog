import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { AngularEditorModule } from '@kolkov/angular-editor';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {
  NavBarComponent,
  PostListComponent,
  LoginComponent,
  PostCreateComponent,
  PostDetailsShortComponent,
  ManagementPanelComponent,
  PostEditorComponent,
  PostEditComponent,
  PostDetailsComponent,
  ConfirmDialogComponent,
  LoadingComponent,
  CommentCreateComponent,
  CommentListComponent
} from './components/blog';
import {
  PostService,
  AuthService,
  TokenService,
  BlogRouterService,
  ConfirmDialogService,
  LoadingService,
  CommentService
} from './services';
import { PostResolver } from './resolvers/post.resolver';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticatedActivator } from './activators/authenticated.activator';
import {
  TokenInterceptor,
  PathInterceptor,
  UnauthorizedInterceptor
} from './interceptors';
import { PostListResolver } from './resolvers/post-list.resolver';
import {
  AboutMeWelcomeComponent,
  AboutMeDescriptionComponent,
  AboutMeEducationComponent,
  AboutMeSkillsComponent,
  AboutMeExperienceComponent,
  AboutMeProfilesComponent,
  AboutMeSkillsListComponent,
  AboutMeComponent
} from './components/about-me';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    PostListComponent,
    AboutMeComponent,
    PostDetailsShortComponent,
    LoginComponent,
    PostCreateComponent,
    ManagementPanelComponent,
    PostEditorComponent,
    PostEditComponent,
    PostDetailsComponent,
    ConfirmDialogComponent,
    AboutMeWelcomeComponent,
    AboutMeDescriptionComponent,
    AboutMeEducationComponent,
    AboutMeSkillsComponent,
    AboutMeSkillsListComponent,
    AboutMeExperienceComponent,
    AboutMeProfilesComponent,
    LoadingComponent,
    CommentCreateComponent,
    CommentListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
    AngularEditorModule,
    ReactiveFormsModule,
    BrowserAnimationsModule
  ],
  providers: [
    PostService,
    CommentService,
    AuthService,
    TokenService,
    PostResolver,
    PostListResolver,
    BlogRouterService,
    AuthenticatedActivator,
    ConfirmDialogService,
    LoadingService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: PathInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: UnauthorizedInterceptor,
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
