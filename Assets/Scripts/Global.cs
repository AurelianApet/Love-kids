using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Global
{
    public static int chat_type;	// if chat_type is 1, private chat, 2 is public chat 
    public static bool isQuit;
    public static string connectionStatus = "Not connected";
    public static List<Contact> contactList;
    public static List<User> userList;
}

public class Contact
{    
    public string user_id;
    public string username;    
    public Contact(string id, string username) {
        this.user_id = id;
		this.username = username;        
    }
}

public class User {
    public string user_id;
    public string status;
	public string user_name;
	public User(string id, string status, string user_name)
    {
        this.user_id = id;
        this.status = status;
		this.user_name = user_name;
    }
}
