import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SignoutRedirectCallbackComponent } from './home/components/signout-redirect-callback.component';
import { WebsiteComponent } from './home/components/websites/website.component';
import { HomeComponent } from './home/components/homes/home.component';
import { SigninRedirectCallbackComponent } from './home/components/signin-redirect-callback.component';
import { UnauthorizedComponent } from './home/components/unauthorized.component';
import { AuthGuardService } from './core/auth-guard.service';
import { RegistrationComponent } from './registrations/registraiton.component';
import { ChatComponent } from './chats/chat.component';

const routes: Routes = [
    { path: '', component: WebsiteComponent },
    { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
    { path: 'chat/:id', component: ChatComponent, canActivate: [AuthGuardService] },
    { path: 'register', component: RegistrationComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'unauthorized', component: UnauthorizedComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
