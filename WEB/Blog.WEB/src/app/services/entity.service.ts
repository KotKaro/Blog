import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

import { Entity } from '../models/entity';

export abstract class EntityService<TEntity extends Entity> {
    constructor(protected endpoint: string, protected httpClient: HttpClient, private toastr: ToastrService) {
        if (endpoint === null || endpoint.trim() === '') {
            throw new Error("Endpoint value hasn't been provided!");
        }
    }

    getAll(pageNumber: number = 1, pageSize: number = 10): Observable<TEntity[]> {
        return this.httpClient.get<TEntity[]>(`/api/${this.endpoint}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    }

    create(entity: TEntity): Observable<Object> {
        return this.httpClient.post(`/api/${this.endpoint}`, entity)
            .pipe(tap(() => {
                this.toastr.success('Created');
            }));
    }

    getById(id: string): Observable<TEntity> {
        return this.httpClient.get<TEntity>(`/api/${this.endpoint}/${id}`);
    }

    update(entity: TEntity): Observable<Object> {
        if (!entity) {
            return of(null);
        }

        return this.httpClient.patch(`/api/${this.endpoint}`, entity)
            .pipe(tap(() => {
                this.toastr.success('Updated');
            }));
    }
}
