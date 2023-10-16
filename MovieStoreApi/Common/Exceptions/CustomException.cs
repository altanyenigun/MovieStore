using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreApi.Common.Exceptions
{
    // base class to use to create CustomError
    public class CustomException : Exception
    {
        public int ErrorCode { get; }
        public string Error { get; }

        public CustomException(int errorCode, string error, string message) : base(message)
        {
            ErrorCode = errorCode;
            Error = error;
        }
    }
    // Creating special error definitions for error situations that may occur in the flow
    public static class CustomExceptions
    {
        public static CustomException ACTOR_NOT_FOUND = new CustomException(700, "ACTOR_NOT_FOUND", "There is no actor with this id.");
        public static CustomException CANT_DELETE_ACTOR = new CustomException(701,"CANT_DELETE_ACTOR","You Cant delete this actor!, You should delete the movies it is linked to!.");

        public static CustomException DIRECTOR_NOT_FOUND = new CustomException(800, "DIRECTOR_NOT_FOUND", "There is no director with this id.");
        public static CustomException CANT_DELETE_DIRECTOR = new CustomException(801,"CANT_DELETE_DIRECTOR","You cant delete this Director!, first you should delete him/her movies!");
        
        public static CustomException MOVIE_NOT_FOUND = new CustomException(900, "MOVIE_NOT_FOUND", "There is no movie with this id.");
        public static CustomException MOVIE_NAME_ALREADY_IN_USE = new CustomException(901, "MOVIE_NAME_ALREADY_IN_USE", "Movie Name already registered.");

        public static CustomException LOGIN_FAILED = new CustomException(1000, "LOGIN_FAILED", "Wrong Username or Password!");
        public static CustomException USERNAME_ALREADY_IN_USE = new CustomException(1001, "USERNAME_ALREADY_IN_USE", "Username already in use!");

        public static CustomException ALREADY_HAVE_THIS_MOVIE = new CustomException(1100, "ALREADY_HAVE_THIS_MOVIE", "User already bought this movie!");

        public static CustomException GENRE_NOT_FOUND = new CustomException(1200, "GENRE_NOT_FOUND", "There is no genre with this id.");
        public static CustomException ALREADY_HAVE_THIS_GENRE = new CustomException(1201, "ALREADY_HAVE_THIS_GENRE", "User already added this genre him/his favorites!");

    }
}