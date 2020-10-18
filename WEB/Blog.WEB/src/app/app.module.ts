import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { PostListComponent } from './components/post-list/post-list.component';
import { AboutMeComponent } from './components/about-me/about-me.component';
import { PostService } from './services/post.service';
import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './services/auth.service';
import { TokenService } from './services/token.service';
import { ToastrModule } from 'ngx-toastr';
import { PostCreateComponent } from './components/post-create/post-create.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PostDetailsShortComponent } from './components/post-details-short/post-details-short.component';
import { ManagementPanelComponent } from './components/managment-panel/management-panel.component';
import { PostEditorComponent } from './components/post-editor/post-editor.component';
import { PostEditComponent } from './components/post-edit/post-edit.component';
import { PostResolver } from './resolvers/post.resolver';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PostDetailsComponent } from './components/post-details/post-details.component';
import { BlogRouterService } from './services/blog-router.service';
import { AuthenticatedActivator } from './activators/authenticated.activator';
import { ConfirmDialogService } from './services/confirm-dialog.service';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { PostListResolver } from './resolvers/post-list.resolver';
import { AboutMeWelcomeComponent } from './components/about-me/about-me-welcome/about-me-welcome.component';
import { AboutMeDescriptionComponent } from './components/about-me/about-me-description/about-me-description.component';
import { AboutMeEducationComponent } from './components/about-me/about-me-education/about-me-education.component';
import { AboutMeSkillsComponent } from './components/about-me/about-me-skills/about-me-skills.component';
import { AboutMeExperienceComponent } from './components/about-me/about-me-experience/about-me-experience.component';
import { AboutMeProfilesComponent } from './components/about-me/about-me-profiles/about-me-profiles.component';

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
    AboutMeExperienceComponent,
    AboutMeProfilesComponent
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
    AuthService,
    TokenService,
    PostResolver,
    PostListResolver,
    BlogRouterService,
    AuthenticatedActivator,
    ConfirmDialogService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
