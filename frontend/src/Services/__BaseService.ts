import axios, { Axios } from 'axios'

import { ServiceConfig, servicesConfig } from './servicesConfig'

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

    constructor(name: keyof typeof servicesConfig) {
        this.config = servicesConfig[name]
        this.client = axios.create({ baseURL: this.config.BASE_URL })
        this.client.defaults.headers.common = {
            accept: 'application/json',
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
