import axios, { Axios } from 'axios'

import { ServiceConfig } from './config'

type RequestOptions = RequestInit & {
    method?: 'GET'
        | 'DELETE'
        | 'HEAD'
        | 'OPTIONS'
        | 'POST'
        | 'PUT'
        | 'PATCH'
        | 'PURGE'
        | 'LINK'
        | 'UNLINK'
    headers?: Record<string, string>
}

export class __BaseService {
    private config: ServiceConfig
    private client: Axios

    constructor(config: ServiceConfig) {
        this.config = config
        this.client = axios.create({ baseURL: this.config.BASE_URL })
        this.client.defaults.headers.common = {
            Accept: 'application/json',
            Authorization: `Bearer ${config.accessToken}`,
            ...this.config.headers,
        }
    }

    private async request(path: string, options?: RequestOptions) {
        const { data } = await this.client.request({
            method: options?.method,
            headers: options?.headers,
            withCredentials: options?.credentials === "include",
            url: path
        })
        return data
    }

    protected get(path: string, options?: RequestOptions) {
        return this.request(path, { ...options, method: 'GET' })
    }
}
