import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { BlogComponent } from './components/blog/blog.component';
import { PostListComponent } from './components/post-list/post-list.component';
import { AboutMeComponent } from './components/about-me/about-me.component';
import { PostService } from './services/post.service';
import { PostDetailsComponent } from './components/post-details/post-details.component';
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

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    BlogComponent,
    PostListComponent,
    AboutMeComponent,
    PostDetailsShortComponent,
    LoginComponent,
    PostCreateComponent,
    ManagementPanelComponent,
    PostEditorComponent,
    PostEditComponent,
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
  providers: [PostService, AuthService, TokenService, PostResolver],
  bootstrap: [AppComponent]
})
export class AppModule { }
