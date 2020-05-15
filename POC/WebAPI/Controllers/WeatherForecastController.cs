using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.TaskScheduler;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers.Common;
using WebAPI.Models;
using System.Net.Http;
using System.Net;

namespace WeAPI.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        // public IEnumerable<WeatherForecast> Get()
        // {
        //     var rng = new Random();
        //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //     {
        //         Date = DateTime.Now.AddDays(index),
        //         TemperatureC = rng.Next(-20, 55),
        //         Summary = Summaries[rng.Next(Summaries.Length)]
        //     })
        //     .ToArray();
        // }
        [HttpGet]
        public string Get(){

            return Processes.GetProcessResult().ToString();
        }

        [HttpPost]
        [Route("~/api/Schedular/Create")]
        public IActionResult CreateSchedular(TaskDetails taskDetails){
            try{
                //return Processes.GetProcessResult("param").ToString();
                // Get the service on the local machine
                using (TaskService ts = new TaskService())
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Does something";

                    // Create a trigger that will fire the task at this time every other day
                    // td.Triggers.Add(new WeeklyTrigger { DaysOfWeek = DaysOfTheWeek.AllDays });
                    // td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });
                    // td.Triggers.Add(new MonthlyTrigger { DaysOfMonth = new int[]{1,2,3} });
                    //td.Triggers.Add(new TimeTrigger { StartBoundary = new DateTime(2020, 5, 15, 9, 0, 0) });

                    td.Triggers.Add(new TimeTrigger{StartBoundary = taskDetails.TimeToSchedule});

                    // Create an action that will launch Notepad whenever the trigger fires
                    //td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));
                    td.Actions.Add(new ExecAction(taskDetails.TaskPath));

                    // Register the task in the root folder
                    
                    if(ts != null){
                        
                        var result = string.Empty;
                        foreach(var t in ts.AllTasks.ToList()){
                            result += t.Name;
                            if(t.Name == taskDetails.TaskName){
                                /*Task available already*/
                                return Conflict("Task Available Already /\n" + result + " \n" + taskDetails.TaskName + " \n" + t.Name);
                            }
                        }
                        ts.RootFolder.RegisterTaskDefinition(taskDetails.TaskName, td);
                        //return "Task created successfully " + ts.RootFolder + ", " + result;
                        /*Task created successfully*/
                        return Ok("201");
                    }
                    return Conflict("Task Definition returns null");
                    
                    // Remove the task we just created
                    //ts.RootFolder.DeleteTask("Test");
                }
            }
            catch(Exception ex){
                return BadRequest(ex.ToString());
            }
        }
    }

}
