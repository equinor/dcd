import axios, { Axios, AxiosRequestConfig, ResponseType } from "axios"

import { ServiceConfig } from "./config"

type RequestOptions = {
    credentials?: RequestCredentials
    headers?: Record<string, string>
    method?: "GET"
        | "DELETE"
        | "HEAD"
        | "OPTIONS"
        | "POST"
        | "PUT"
        | "PATCH"
        | "PURGE"
        | "LINK"
        | "UNLINK"
    body?: Record<string, any>
}

export class __BaseService {
    private config: ServiceConfig
    private client: Axios

    constructor(config: ServiceConfig) {
        this.config = config
        this.client = axios.create({ baseURL: this.config.BASE_URL })
        this.client.defaults.headers.common = {
            Accept: "application/json",
            Authorization: `Bearer ${config.accessToken}`,
            ...this.config.headers,
        }
    }

    private async request(path: string, options?: RequestOptions): Promise<any> {
        const { data } = await this.client.request({
            method: options?.method,
            headers: options?.headers,
            withCredentials: options?.credentials === "include",
            url: path,
            data: options?.body,
        })
        return data
    }

    private async requestExcel(path: string, responseType?: ResponseType, options?: RequestOptions): Promise<any> {
        const { data } = await this.client.request({
            method: options?.method,
            headers: options?.headers,
            withCredentials: options?.credentials === "include",
            url: path,
            data: options?.body,
            responseType: responseType ?? "text",
        })
        return data
    }

    protected async postWithParams(
        path: string,
        options?: RequestOptions,
        requestQuery?: AxiosRequestConfig,
    ): Promise<any> {
        const { data } = await this.client.post(path, options?.body, requestQuery)

        return data
    }

    protected postExcel<T = any>(path: string, responseType?: ResponseType, options?: RequestOptions): Promise<T> {
        return this.requestExcel(path, responseType, { ...options, method: "POST" })
    }

    protected get<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "GET" })
    }

    protected post<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "POST" })
    }

    protected put<T = any>(path: string, options?: RequestOptions): Promise<T> {
        return this.request(path, { ...options, method: "PUT" })
    }
}
