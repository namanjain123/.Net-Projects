﻿using Domain_Layer.Data;
using Domain_Layer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.Interface;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsController : ControllerBase { 
    private readonly ICustomer<Student> _customService;
    private readonly AppDbContext _applicationDbContext;
    public DetailsController(ICustomer<Student> customService, AppDbContext applicationDbContext)
    {
        _customService = customService;
        _applicationDbContext = applicationDbContext;
    }
    [HttpGet(nameof(GetStudentById))]
    public IActionResult GetStudentById(int Id)
    {
        var obj = _customService.Get(Id);
        if (obj == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(obj);
        }
    }
    [HttpGet(nameof(GetAllStudent))]
    public IActionResult GetAllStudent()
    {
        var obj = _customService.GetAll();
        if (obj == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(obj);
        }
    }
    [HttpPost(nameof(CreateStudent))]
    public IActionResult CreateStudent(Student student)
    {
        if (student != null)
        {
            _customService.Insert(student);
            return Ok("Created Successfully");
        }
        else
        {
            return BadRequest("Somethingwent wrong");
        }
    }
    [HttpPost(nameof(UpdateStudent))]
    public IActionResult UpdateStudent(Student student)
    {
        if (student != null)
        {
            _customService.Update(student);
            return Ok("Updated SuccessFully");
        }
        else
        {
            return BadRequest();
        }
    }
    [HttpDelete(nameof(DeleteStudent))]
    public IActionResult DeleteStudent(Student student)
    {
        if (student != null)
        {
            _customService.Delete(student);
            return Ok("Deleted Successfully");
        }
        else
        {
            return BadRequest("Something went wrong");
        }
    }
}
}
