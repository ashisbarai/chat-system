import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class AppconfigService {
	clientRoot : string;
	apiRoot : string;
	stsAuthority : string;
  clientId : string;
  
  constructor() { 
    this.clientRoot = environment.production ? 'http://localhost:4200/' : 'http://localhost:4200/';
    this.apiRoot = environment.production ? 'https://localhost:44391/api/' : 'https://localhost:44391/api/';
    this.stsAuthority = environment.production ? 'https://localhost:44368/' : 'https://localhost:44368/';
    this.clientId = 'chat-spa-client';
  }
}
