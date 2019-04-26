////////////////////////////////////////////////////////////////////////////
// NavigatorServer.cs - File Server for WPF NavigatorClient Application   //
// ver 2.0                                                                //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017        //
////////////////////////////////////////////////////////////////////////////
/*//Author:      Siddharth Joshi, sijoshi@syr.edu                          //
// Source:      Jim Fawcett, Syracuse University, CST 4-187              //
//              jfawcett@twcny.rr.com                                    //
///////////////////////////////////////////////////////////////////////////
 * Package Operations:
 * -------------------
 * This package defines a single NavigatorServer class that returns file
 * and directory information about its rootDirectory subtree.  It uses
 * a message dispatcher that handles processing of all incoming and outgoing
 * messages.
 * 
 * Maintanence History:
 * --------------------
 * 
 * ver 1.0 - 1st December 2018
 * - first release
 * ver 1.0 - 5th December 2018
 * - Added Functionality for communication for
 * 1. Tokenizer
 * 2. Semi Expression
 * 3. Type Table Analysis
 * 4. Dependency Analysis
 * 5. Strong Component
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePassingComm;
using CodeAnalysis;
using System.IO;

namespace Navigator
{
  public class NavigatorServer
  {
    IFileMgr localFileMgr { get; set; } = null;
    Comm comm { get; set; } = null;

    Dictionary<string, Func<CommMessage, CommMessage>> messageDispatcher = 
      new Dictionary<string, Func<CommMessage, CommMessage>>();

    /*----< initialize server processing >-------------------------*/

    public NavigatorServer()
    {
      initializeEnvironment();
      Console.Title = "Navigator Server";
      localFileMgr = FileMgrFactory.create(FileMgrType.Local);
    }
    /*----< set Environment properties needed by server >----------*/

    void initializeEnvironment()
    {
      Environment.root = ServerEnvironment.root;
      Environment.address = ServerEnvironment.address;
      Environment.port = ServerEnvironment.port;
      Environment.endPoint = ServerEnvironment.endPoint;
    }
    /*----< define how each message will be processed >------------*/

    void initializeDispatcher()
    {
			

      Func<CommMessage, CommMessage> getTopFiles = (CommMessage msg) =>
      {
        localFileMgr.currentPath = "";
        CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
        reply.to = msg.from;
        reply.from = msg.to;
        reply.command = "getTopFiles";
        reply.arguments = localFileMgr.getFiles().ToList<string>();
        return reply;
      };
      messageDispatcher["getTopFiles"] = getTopFiles;

      Func<CommMessage, CommMessage> getTopDirs = (CommMessage msg) =>
      {
        localFileMgr.currentPath = "";
        CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
        reply.to = msg.from;
        reply.from = msg.to;
        reply.command = "getTopDirs";
        reply.arguments = localFileMgr.getDirs().ToList<string>();
        return reply;
      };
      messageDispatcher["getTopDirs"] = getTopDirs;

            Executive exe = new Executive();

            Func<CommMessage, CommMessage> moveIntoFolderFiles = (CommMessage msg) =>
      {
        if (msg.arguments.Count() == 1)
          localFileMgr.currentPath = msg.arguments[0];
        CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
        reply.to = msg.from;
        reply.from = msg.to;
        reply.command = "moveIntoFolderFiles";
        reply.arguments = localFileMgr.getFiles().ToList<string>();
        return reply;
      };
      messageDispatcher["moveIntoFolderFiles"] = moveIntoFolderFiles;

      Func<CommMessage, CommMessage> moveIntoFolderDirs = (CommMessage msg) =>
      {
        if (msg.arguments.Count() == 1)
          localFileMgr.currentPath = msg.arguments[0];
        CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
        reply.to = msg.from;
        reply.from = msg.to;
        reply.command = "moveIntoFolderDirs";
        reply.arguments = localFileMgr.getDirs().ToList<string>();
        return reply;
      };
      messageDispatcher["moveIntoFolderDirs"] = moveIntoFolderDirs;

		
			
			Func<CommMessage, CommMessage> getTypeAnalysis = (CommMessage msg) =>
			{
				localFileMgr.currentPath = "";
				CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
				reply.to = msg.from;
				reply.from = msg.to;
				reply.command = "getTypeAnalysis";
				string Displaytokenfile = Path.GetFullPath(ServerEnvironment.displaypath + "SampleTypeAnalysis.txt");
				exe.displaytypeanalysis(msg.arguments, Displaytokenfile);
				string[] displayfile = { Displaytokenfile };
				reply.arguments = displayfile.ToList();
				return reply;
			};
			messageDispatcher["getTypeAnalysis"] = getTypeAnalysis;
			Func<CommMessage, CommMessage> DependencyAnalysis = (CommMessage msg) =>
			{
				localFileMgr.currentPath = "";
				CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
				reply.to = msg.from;
				reply.from = msg.to;
				reply.command = "getDependency";
				string Displaytokenfile = Path.GetFullPath(ServerEnvironment.displaypath + "SampleDependency.txt");
				exe.displaydependency(msg.arguments, Displaytokenfile);
				string[] displayfile = { Displaytokenfile };
				reply.arguments = displayfile.ToList();
				return reply;
			};
			messageDispatcher["getDependency"] = DependencyAnalysis;
			
			Func<CommMessage, CommMessage> StrongComponent = (CommMessage msg) =>
			{
				localFileMgr.currentPath = "";
				CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
				reply.to = msg.from;
				reply.from = msg.to;
				reply.command = "getStrongComponent";
				string Displaytokenfile = Path.GetFullPath(ServerEnvironment.displaypath + "SampleStrongComponent.txt");
				exe.displaystrongcomponent(msg.arguments, Displaytokenfile);
				string[] displayfile = { Displaytokenfile };
				reply.arguments = displayfile.ToList();
				return reply;
			};
			messageDispatcher["getStrongComponent"] = StrongComponent;

            Func<CommMessage, CommMessage> Tokenizer = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getTokenizer";
                string Displaytokenfile = Path.GetFullPath(ServerEnvironment.displaypath + "SampleToken.txt");
                exe.displaytok(msg.arguments, Displaytokenfile);
                string[] displayfile = { Displaytokenfile };
                reply.arguments = displayfile.ToList();

                return reply;
            };

            messageDispatcher["getTokenizer"] = Tokenizer;
            Func<CommMessage, CommMessage> SemiExpression = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getSemiExp";
                string Displaytokenfile = Path.GetFullPath(ServerEnvironment.displaypath + "SampleSemiExp.txt");
                exe.displaysemi(msg.arguments, Displaytokenfile);
                string[] displayfile = { Displaytokenfile };
                reply.arguments = displayfile.ToList();
                return reply;
            };
            messageDispatcher["getSemiExp"] = SemiExpression;

        }
		/*----< Server processing >------------------------------------*/
		/*
		 * - all server processing is implemented with the simple loop, below,
		 *   and the message dispatcher lambdas defined above.
		 */
		static void Main(string[] args)
    {
      TestUtilities.title("Starting Navigation Server", '=');
      try
      {
        NavigatorServer server = new NavigatorServer();
        server.initializeDispatcher();
        server.comm = new MessagePassingComm.Comm(ServerEnvironment.address, ServerEnvironment.port);
        
        while (true)
        {
          CommMessage msg = server.comm.getMessage();
          if (msg.type == CommMessage.MessageType.closeReceiver)
            break;
          msg.show();
          if (msg.command == null)
            continue;
          CommMessage reply = server.messageDispatcher[msg.command](msg);
          reply.show();
          server.comm.postMessage(reply);
        }
      }
      catch(Exception ex)
      {
        Console.Write("\n  exception thrown:\n{0}\n\n", ex.Message);
      }
    }
  }
}
