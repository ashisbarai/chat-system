import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './shared/material.module';
import { WebsiteComponent } from './home/components/websites/website.component';
import { HomeComponent } from './home/components/homes/home.component';
import { HomeService } from './home/components/homes/home.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ChatService } from './chats/chat.service';
import { RegistrationComponent } from './registrations/registraiton.component';
import { UserService } from './users/user.service';
import { ChatComponent } from './chats/chat.component';

@NgModule({
  declarations: [
    AppComponent,
    WebsiteComponent,
    HomeComponent,
    RegistrationComponent,
    ChatComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    FontAwesomeModule,
    HttpClientModule,
    CoreModule,
    AppRoutingModule,
    NgbModule,
    BrowserAnimationsModule,
    MaterialModule
  ],
  providers: [HomeService, ChatService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
