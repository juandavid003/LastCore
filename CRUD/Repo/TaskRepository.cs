using System;
using System.Collections.Generic;
using System.Linq;
using DB;

public class TaskRepository
{
    private readonly DB_CrudLogInEntities _dbContext;

    public TaskRepository(string connectionString)
    {
        _dbContext = new DB_CrudLogInEntities(connectionString);
    }

    public IEnumerable<tarea> GetAllTasks()
    {
        return _dbContext.tareas.ToList();
    }

    public tarea GetTaskById(int id)
    {
        return _dbContext.tareas.FirstOrDefault(t => t.task_id == id);
    }

    public void AddTask(tarea newTask)
    {
        _dbContext.tareas.Add(newTask);
        _dbContext.SaveChanges();
    }

    public bool UpdateTask(int id, tarea updatedTask)
    {
        var existingTask = _dbContext.tareas.FirstOrDefault(t => t.task_id == id);
        if (existingTask == null)
            return false;

        existingTask.taskname = updatedTask.taskname;
        existingTask.start_date = updatedTask.start_date;
        existingTask.estimated_time = updatedTask.estimated_time;
        existingTask.employee = updatedTask.employee;
        existingTask.project = updatedTask.project;

        _dbContext.SaveChanges();
        return true;
    }

    public bool DeleteTask(int id)
    {
        var task = _dbContext.tareas.FirstOrDefault(t => t.task_id == id);
        if (task == null)
            return false;

        _dbContext.tareas.Remove(task);
        _dbContext.SaveChanges();
        return true;
    }

    public IEnumerable<tarea> GetTasksByDate(DateTime startDate, DateTime endDate)
    {
        return _dbContext.tareas
            .Where(t => t.start_date >= startDate && t.start_date <= endDate)
            .ToList();
    }
}
