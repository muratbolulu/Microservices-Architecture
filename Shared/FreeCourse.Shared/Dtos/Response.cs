using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        //for success
        public T Data { get; private set; }

        [JsonIgnore]
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        //for errors
        public List<string> Errors { get; set; }

        //Static Factory Method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = false };
        }

        //for errors
        public static Response<T> Fail(List<string> erorrs, int statusCode)
        {
            return new Response<T>
            {
                Errors = erorrs,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = new List<string>() { errors },
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
