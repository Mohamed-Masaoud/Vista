﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vista.Api.Data;

namespace Vista.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly TrainersDbContext _context;

        public SessionsController(TrainersDbContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
          if (_context.Sessions == null)
          {
              return NotFound();
          }
            return await _context.Sessions.ToListAsync();
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(int id)
        {
          if (_context.Sessions == null)
          {
              return NotFound();
          }
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return session;
        }

        // GET:api/Sessions
        /*[HttpGet("GetFreeSessions")]
        public async Task<ActionResult<IEnumerable<Session>>> GetFreeSessions(String CategoryCode, DateTime date)
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            var freeSessions = _context.Sessions.Select(c =>
                new
                {
                    c.SessionId,
                    c.SessionDate,
                    TrainerName = c.Trainer.Name,
                    
                }
            )
        }*/

        // PUT: api/Sessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(int id, Session session)
        {
            if (id != session.SessionId)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sessions/AddSessionsDate
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddSessionDate")]
        public async Task<ActionResult<Session>> AddSessionDate(Session session)
        {
          if (_context.Sessions == null)
          {
              return Problem("Entity set 'TrainersDbContext.Sessions'  is null.");
          }
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSession", new { id = session.SessionId }, session);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            if (_context.Sessions == null)
            {
                return NotFound();
            }
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(int id)
        {
            return (_context.Sessions?.Any(e => e.SessionId == id)).GetValueOrDefault();
        }
    }
}
